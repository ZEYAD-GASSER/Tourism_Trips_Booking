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
            var trip = _context.Trips
                .Include(t => t.Bookings)
                .FirstOrDefault(t => t.Id == id);

            if (trip == null)
                return NotFound();

            var reviews = _context.ReviewAndRating
                .Where(r => r.TripID == id)
                .Include(r => r.UserAccount)
                .ToList();

            ViewBag.AverageRating = reviews.Any() ? reviews.Average(r => r.Rating ?? 0) : 0;
            ViewBag.Reviews = reviews;

            var userId = HttpContext.Session.GetInt32("UserId");
            bool hasBooked = false;

            if (userId != null)
            {
                hasBooked = _context.Booking.Any(b => b.UserID == userId && b.TripID == id);
            }

            ViewBag.HasBooked = hasBooked;

            return View(trip);
        }

        [HttpPost]

        public IActionResult AddReview(ReviewAndRating review)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId == null)
                return Unauthorized(); 

  
            bool hasBooked = _context.Booking.Any(b => b.UserID == userId && b.TripID == review.TripID);
            if (!hasBooked)
                return Forbid();  

            review.UserID = userId.Value;

            _context.ReviewAndRating.Add(review);
            _context.SaveChanges();

            return RedirectToAction("Details", new { id = review.TripID });
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
            return View(); 
        }
    }
}
