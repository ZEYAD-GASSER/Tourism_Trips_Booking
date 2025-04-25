using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using System.Linq;
using Tourism_Trips_Booking.ViewModels;
using Microsoft.EntityFrameworkCore;

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
            var users = _context.UserAccount.ToList();
            var bookings = _context.Booking
                          .Include(b => b.Trips)
                          .Include(b => b.UserAccount)
                          .ToList();

            var model = new ManageViewModel
            {
                Users = users,
                Bookings = bookings
            };

            // عرض الرسالة الخاصة بحذف المستخدم أو الحجز
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(model);
        }

        [HttpPost]
        public IActionResult DeleteUser(int id)
        {
            var user = _context.UserAccount.FirstOrDefault(u => u.Id == id);

            if (user == null)
            {
                return NotFound();
            }

            _context.UserAccount.Remove(user);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "User deleted successfully";  // رسالة النجاح
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

            _context.Booking.Remove(booking);
            _context.SaveChanges();

            TempData["SuccessMessage"] = "Booking deleted successfully";  // رسالة النجاح
            return RedirectToAction("Manage");
        }
    }
}
