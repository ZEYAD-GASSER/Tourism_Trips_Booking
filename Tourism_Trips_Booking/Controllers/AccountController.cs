using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Tourism_Trips_Booking.Models;
using Tourism_Trips_Booking.ViewModels;
using System.Linq;

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
        public IActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _context.UserAccount
                    .Where(u => u.Email == model.Email && u.Password == model.Password)
                    .FirstOrDefault();

                if (user != null)
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, user.Email),
                        new Claim("Name", user.Name),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
                    return RedirectToAction("SecurePage");
                }
                else
                {
                    ModelState.AddModelError("", "يحرامي يبن ال");
                }
            }

            return View(model);
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
                    Password = model.Password,
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
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index","Home");
        }

        [Authorize]
        public IActionResult SecurePage()
        {
            ViewBag.Name = HttpContext.User.Identity.Name;
            return View();
        }
    }
}
