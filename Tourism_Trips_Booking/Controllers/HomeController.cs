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
        public IActionResult Index(string searchTerm)
        {
            var trips = _context.Trips.AsQueryable();
            if (!string.IsNullOrEmpty(searchTerm))
            {            
                trips = trips.Where(t => t.Title.Contains(searchTerm) || t.Description.Contains(searchTerm));
            }
            ViewData["SearchTerm"] = searchTerm;
            return View(trips.ToList());
        }
        public IActionResult Details(int id)
        {


            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();
            var reviews = _context.ReviewAndRating
           .Where(r => r.TripID == id)
           .Include(r => r.UserAccount) 
           .ToList();

            ViewBag.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating ?? 0) : 0;
            ViewBag.Reviews = reviews;

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
