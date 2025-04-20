using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tourism_Trips_Booking.Models;

namespace Tourism_Trips_Booking.Controllers
{
    public class HomeController : Controller
    {
        
        private readonly Tourism_Trips_Booking_Context _context;
        public HomeController(Tourism_Trips_Booking_Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var trips = _context.Trips.ToList();


            return View(trips);
        }
        public IActionResult Details(int id)
        {


            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            return View(trip);
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
