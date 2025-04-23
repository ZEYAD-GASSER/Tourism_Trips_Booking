using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tourism_Trips_Booking.Models;
using Tourism_Trips_Booking.ViewModels;
using System.Linq;
using Microsoft.AspNetCore.Identity;

namespace Tourism_Trips_Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;

        public AccountController(Tourism_Trips_Booking_Context context)
        {
            _context = context;
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
                        HttpContext.Session.SetString("UserRole", user.Role);
                        HttpContext.Session.SetString("UserName", user.Name);
                        HttpContext.Session.SetInt32("UserId", user.Id);

                        var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim("Name", user.Name),
                    new Claim(ClaimTypes.Role, user.Role),
                };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));

                        if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                            return Redirect(returnUrl);
                        else if (user.Role == "Admin")
                            return RedirectToAction("AdminDashboard", "Account");
                        else
                            return RedirectToAction("Index", "Home");
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
        public IActionResult Registeration(RegisterationViewModels model)
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
                    Role = "User"
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
            HttpContext.Session.SetInt32("UserId", 0);

            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
        public IActionResult AdminDashboard()
        {
            var role = HttpContext.Session.GetString("UserRole");

            if (role != "Admin")
                return View("Index", "Home");

            // AdminDashboard
            else
            {
                var Trips = _context.Trips.ToList();
                return View("AdminDashboard", Trips);
            }
        }




    }
}
