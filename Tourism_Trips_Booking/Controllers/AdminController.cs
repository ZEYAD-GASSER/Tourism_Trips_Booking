using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using System.Linq;
using Tourism_Trips_Booking.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Tourism_Trips_Booking.Controllers
{
    public class AdminController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;

        public AdminController(Tourism_Trips_Booking_Context context)
        {
            _context = context;
        }

        public IActionResult Manage()
        {
            var currentUserIdStr = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Fallback: if user not logged in or claim is missing, return all users
            if (string.IsNullOrEmpty(currentUserIdStr))
            {
                currentUserIdStr = "-1"; // dummy value that doesn't match any user
            }

            int currentUserId = int.Parse(currentUserIdStr);

            var users = _context.UserAccount
                                .Where(u => u.Id != currentUserId)
                                .ToList();

            var bookings = _context.Booking
                          .Include(b => b.Trips)
                          .Include(b => b.UserAccount)
                          .ToList();

            var model = new ManageViewModel
            {
                Users = users,
                Bookings = bookings
            };

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(model);
        }


        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.UserAccount.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                TempData["SuccessMessage"] = "User not found.";
                return RedirectToAction("Manage");
            }

            if (user.Role != null && user.Role.ToLower() == "admin")
            {
                TempData["SuccessMessage"] = "You cannot delete an admin account.";
                return RedirectToAction("Manage");
            }

            var userBookings = _context.Booking.Where(b => b.UserID == id).ToList();

            foreach (var booking in userBookings)
            {
                var payments = _context.Payment.Where(p => p.BookingID == booking.Id).ToList();
                _context.Payment.RemoveRange(payments);
            }

            _context.Booking.RemoveRange(userBookings);

            var reviews = _context.ReviewAndRating.Where(r => r.UserID == id).ToList();
            _context.ReviewAndRating.RemoveRange(reviews);

            _context.UserAccount.Remove(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User deleted successfully.";
            return RedirectToAction("Manage");
        }




        [HttpPost]
        public IActionResult DeleteBooking(int id)
        {
            var booking = _context.Booking.FirstOrDefault(b => b.Id == id);

            if (booking == null)
            {
                return NotFound();
            }

            var payments = _context.Payment.Where(p => p.BookingID == id).ToList();
            _context.Payment.RemoveRange(payments);

            // Then delete the booking
            _context.Booking.Remove(booking);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Booking deleted successfully";
            return RedirectToAction("Manage");
        }
    }
}
