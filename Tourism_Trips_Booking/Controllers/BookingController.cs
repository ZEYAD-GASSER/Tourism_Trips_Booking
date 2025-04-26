using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
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

        public IActionResult Checkout(int tripId)
        {
            if (!User.Identity.IsAuthenticated)
            {
                var returnUrl = Url.Action("Details", "Home", new { id = tripId });
                return RedirectToAction("Login", "Account", new { returnUrl });
            }

            var trip = _context.Trips.FirstOrDefault(t => t.Id == tripId);
            if (trip == null) return NotFound();

            return View("SelectPayment", trip);
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmPayment(int tripId, int numberOfPeople, string paymentMethod)
        {
            if (!User.Identity.IsAuthenticated)
                return RedirectToAction("Login", "Account");

            var trip = await _context.Trips.FindAsync(tripId);
            if (trip == null) return NotFound();

            var userEmail = User.Identity?.Name;
            if (userEmail == null) return Unauthorized();

            var user = await _context.UserAccount.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user == null) return Unauthorized();

            // Calculate the total price based on the number of people
            var totalPrice = trip.price * numberOfPeople;

            var booking = new Booking
            {
                TripID = tripId,
                UserID = user.Id,
                Trips = trip,
                UserAccount = user
            };

            _context.Booking.Add(booking);
            await _context.SaveChangesAsync();

            var payment = new Payment
            {
                BookingID = booking.Id,
                Booking = booking,
                Price = totalPrice,  // Use the total price here
                CashOrCard = paymentMethod
            };

            _context.Payment.Add(payment);
            await _context.SaveChangesAsync();

            // Save total price in TempData (convert to string for TempData serialization)
            TempData["TripTitle"] = trip.Title;
            TempData["PaymentMethod"] = paymentMethod;
            TempData["OrderId"] = booking.Id;
            TempData["TotalPrice"] = totalPrice.ToString("F2");  // Convert to string for TempData

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            // Convert TotalPrice back to double
            var totalPrice = TempData["TotalPrice"] != null ? Convert.ToDouble(TempData["TotalPrice"]) : 0.0;

            ViewBag.TotalPrice = totalPrice;
            return View();
        }
    }
}
