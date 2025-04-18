using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using Tourism_Trips_Booking.Services;

namespace Tourism_Trips_Booking.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserAccountService _userService;

        public AccountController(UserAccountService userService)
        {
            _userService = userService;
        }

        public IActionResult Login() => View();

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            var user = _userService.Login(email, password);
            if (user != null)
            {
                TempData["UserName"] = user.Name;
                TempData["UserRole"] = user.Role;

                return RedirectToAction("Index", "Dashboard"); // بدل ما يرجع للـ Home، يرجع للـ Dashboard مثلاً
            }

            ViewBag.Message = "Invalid email or password";
            return View();
        }

        public IActionResult Register() => View();

        [HttpPost]
        public IActionResult Register(UserAccount user)
        {
            _userService.Register(user);
            return RedirectToAction("Login");
        }
        public IActionResult UserChoice()
        {
            return View();
        }

    }
}
