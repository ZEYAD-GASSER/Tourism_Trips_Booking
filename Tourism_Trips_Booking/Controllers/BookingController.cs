using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;

using Tourism_Trips_Booking.ViewModels;

namespace Tourism_Trips_Booking.Controllers
{
    public class BookingController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;
        public BookingController(Tourism_Trips_Booking_Context context)
        {
            _context = context;
        }

        public IActionResult Checkout(int TripId)
        {
            if (!User.Identity.IsAuthenticated)
            {

                var returnUrl = Url.Action("Details", "Home", new { id = TripId });
                return RedirectToAction("Login", "Account", new { returnUrl });
            }
            return View();
        }
    }
}
