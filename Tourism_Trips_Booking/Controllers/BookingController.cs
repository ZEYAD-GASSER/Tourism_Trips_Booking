using Microsoft.AspNetCore.Mvc;

namespace Tourism_Trips_Booking.Controllers
{
    public class BookingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
