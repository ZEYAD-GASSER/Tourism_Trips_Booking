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
            if (trip == null) return NotFound();

            // Get average rating
            var reviews = _context.ReviewAndRating
                .Include(r => r.UserAccount)
                .Where(r => r.TripID == id)
                .ToList();

            var averageRating = reviews.Any() ? reviews.Average(r => r.Rating) : 0;
            ViewBag.AverageRating = averageRating;
            ViewBag.Reviews = reviews;

            // Check if the current user has booked this trip
            var userId = HttpContext.Session.GetInt32("UserId");
            if (userId != null)
            {
                var booking = _context.Booking
                    .Include(b => b.Payment)
                    .FirstOrDefault(b => b.TripID == id && b.UserID == userId);

                if (booking != null)
                {
                    ViewBag.HasBooked = true;
                    // Get the payment information
                    var payment = _context.Payment.FirstOrDefault(p => p.BookingID == booking.Id);
                    ViewBag.PaidAmount = payment?.Price ?? 0;
                }
                else
                {
                    ViewBag.HasBooked = false;
                }
            }

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
