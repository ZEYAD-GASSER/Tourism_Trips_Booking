using Microsoft.AspNetCore.Mvc;

namespace Tourism_Trips_Booking.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // ??? ???? ????? ????? ?? Admin ??? User
            
            return View("ChooseRole");
        }

        public IActionResult AdminLogin()
        {
            return RedirectToAction("Login", "Account");
        }

        public IActionResult UserLogin()
        {
            return RedirectToAction("Login", "Account");
        }
        public IActionResult UserChoice ()
        {
            return View(); // ?????? ????? ??? UserOptions
        }
    }
}
