using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tourism_Trips_Booking.Models;
using Tourism_Trips_Booking.ViewModels;
using System.Linq;
using System.IO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;

namespace Tourism_Trips_Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AccountController(Tourism_Trips_Booking_Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }


        public IActionResult Index()
        {
            return View(_context.UserAccount.ToList());
        }

        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccount.FirstOrDefault(u => u.Email == model.Email);

                if (user != null)
                {
                    var hasher = new PasswordHasher<UserAccount>();
                    var result = hasher.VerifyHashedPassword(null, user.Password, model.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        // Set session variables
                        HttpContext.Session.SetString("UserRole", user.Role);
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetInt32("UserId", user.Id);
                        HttpContext.Session.SetString("Email", user.Email);

                        // Create claims
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.Name, user.Email),
                            new Claim("Name", user.Name),
                            new Claim(ClaimTypes.Role, user.Role),
                            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                        };

                        // Create claims identity
                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        // Create authentication properties
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = true, // This makes the cookie persistent
                            ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Cookie expires in 7 days
                        };

                        // Sign in the user
                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        // Redirect based on role
                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else if (user.Role == "Admin")
                            return RedirectToAction("AdminDashboard", "Account");
                        else
                            return RedirectToAction("UserDashboard", "Account");
                    }
                }

                ModelState.AddModelError("", "Wrong email or password");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }


        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(RegisterationViewModels model)
        {
            if (ModelState.IsValid)
            {
                var existingUser = _context.UserAccount.FirstOrDefault(u => u.Email == model.Email);
                if (existingUser != null)
                {
                    ModelState.AddModelError("", "This email is already registered.");
                    return View("Register", model);
                }

                UserAccount account = new UserAccount
                {
                    Name = model.Name,
                    Email = model.Email,
                    Password = new PasswordHasher<UserAccount>().HashPassword(null, model.Password),
                    Role = "User",
                    ProfilePicture = SaveImage(model.ProfilePicture)
                };

                try
                {
                    _context.UserAccount.Add(account);
                    _context.SaveChanges();
                    ModelState.Clear();
                    ViewBag.Message = $"{account.Name} registered successfully. Please login now!";
                    return View("Register");
                }
                catch (DbUpdateException ex)
                {
                    ModelState.AddModelError("", "An error occurred while saving the account.");
                    return View("Register", model);
                }
            }
            return View("Register", model);
        }

        public IActionResult LogOut()
        {
            // Clear session
            HttpContext.Session.Clear();

            // Sign out
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Clear authentication cookies
            foreach (var cookie in Request.Cookies.Keys)
            {
                Response.Cookies.Delete(cookie);
            }

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
        [Authorize]
        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");
            var userEmail = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(role) || role != "Admin")
            {
                TempData["ErrorMessage"] = "You do not have permission to access the admin dashboard.";
                return RedirectToAction("Index", "Home");
            }

            var trips = _context.Trips.ToList();
            return View(trips);
        }
        public IActionResult UserDashboard()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return RedirectToAction("Login", "Account");

            var now = DateTime.Now;

            // Get all trips with their images
            var allTrips = _context.Trips
                .Include(t => t.Bookings)
                .ThenInclude(b => b.UserAccount)
                .ToList();

            // Get booked trip IDs for the current user
            var bookedTripIds = _context.Booking
                .Where(b => b.UserID == userId)
                .Select(b => b.TripID)
                .ToList();

            // Filter trips based on dates and booking status
            var bookedTrips = allTrips
                .Where(t => bookedTripIds.Contains(t.Id) && t.StartDate > now)
                .ToList();

            var recommendedTrips = allTrips
                .Where(t => !bookedTripIds.Contains(t.Id) && t.StartDate > now)
                .OrderByDescending(t => t.Bookings.Count)
                .Take(5)
                .ToList();

            var previousTrips = allTrips
                .Where(t => bookedTripIds.Contains(t.Id) && t.EndDate < now)
                .ToList();

            ViewBag.BookedTrips = bookedTrips;
            ViewBag.RecommendedTrips = recommendedTrips;
            ViewBag.PreviousTrips = previousTrips;

            return View();
        }
        private string SaveImage(IFormFile file)
        {
            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/images/" + newFileName;
        }
        [Authorize]
        public IActionResult Profile()
        {
            // Try to get user ID from claims first
            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
            
            // If not found in claims, try session
            if (string.IsNullOrEmpty(currentUserIdStr))
            {
                var sessionUserId = HttpContext.Session.GetInt32("UserId");
                if (sessionUserId.HasValue)
                {
                    currentUserIdStr = sessionUserId.ToString();
                }
                else
                {
                    return RedirectToAction("Login", "Account");
                }
            }

            var user = _context.UserAccount.FirstOrDefault(u => u.Id.ToString() == currentUserIdStr);
            if (user == null)
                return NotFound();

            var now = DateTime.Now;
            var previousTripsCount = _context.Booking
                .Count(b => b.UserID == user.Id && b.Trips.EndDate < now);

            var profileViewModel = new UserProfileViewModel
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ExistingProfilePicture = user.ProfilePicture,
                BookingCount = _context.Booking.Count(b => b.UserID == user.Id),
                PreviousTripsCount = previousTripsCount
            };

            return View(profileViewModel);
        }

        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(currentUserIdStr))
                {
                    var sessionUserId = HttpContext.Session.GetInt32("UserId");
                    if (sessionUserId.HasValue)
                    {
                        currentUserIdStr = sessionUserId.ToString();
                    }
                    else
                    {
                        return RedirectToAction("Login", "Account");
                    }
                }

                var user = await _context.UserAccount.FirstOrDefaultAsync(u => u.Id.ToString() == currentUserIdStr);
                if (user == null)
                {
                    return NotFound();
                }

                // Update basic info
                user.Name = model.Name;
                user.Email = model.Email;

                // Handle profile picture update
                if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
                {
                    user.ProfilePicture = SaveImage(model.ProfilePicture);
                }

                // Handle password change
                if (!string.IsNullOrEmpty(model.NewPassword))
                {
                    if (string.IsNullOrEmpty(model.ConfirmNewPassword))
                    {
                        ModelState.AddModelError("ConfirmNewPassword", "Please confirm your new password");
                        return View(model);
                    }

                    if (model.NewPassword != model.ConfirmNewPassword)
                    {
                        ModelState.AddModelError("", "New password and confirmation password do not match");
                        return View(model);
                    }

                    if (model.NewPassword.Length < 6)
                    {
                        ModelState.AddModelError("NewPassword", "Password must be at least 6 characters long");
                        return View(model);
                    }

                    user.Password = new PasswordHasher<UserAccount>().HashPassword(null, model.NewPassword);
                    TempData["SuccessMessage"] = "Profile and password updated successfully!";
                }
                else
                {
                    TempData["SuccessMessage"] = "Profile updated successfully!";
                }

                _context.UserAccount.Update(user);
                await _context.SaveChangesAsync();

                // Update session if needed
                HttpContext.Session.SetString("UserName", user.Name);
                HttpContext.Session.SetString("Email", user.Email);

                return RedirectToAction("Profile");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "An error occurred while updating your profile. Please try again.");
                return View(model);
            }
        }





    }
}
