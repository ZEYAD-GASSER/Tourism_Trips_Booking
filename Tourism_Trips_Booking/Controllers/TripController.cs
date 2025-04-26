using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using System.Threading.Tasks;

namespace Tourism_Trips_Booking.Controllers
{
    public class TripController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;

        public TripController(Tourism_Trips_Booking_Context context)
        {
            _context = context;
        }

        // GET: Trip/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trips trip, IFormFile ImagePath)
        {
            if (ModelState.IsValid)
            {
                if (ImagePath != null && ImagePath.Length > 0)
                {
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", ImagePath.FileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await ImagePath.CopyToAsync(stream);
                    }
                    trip.ImagePath = "/images/" + ImagePath.FileName;
                }
                else
                {
                    trip.ImagePath = "/images/default.jpg";
                }

                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                return RedirectToAction("AdminDashboard", "Account");
            }

            return View(trip);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trips trip, IFormFile ImagePath)
        {
            if (id != trip.Id)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (ImagePath != null)
                    {
                        var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", ImagePath.FileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await ImagePath.CopyToAsync(stream);
                        }
                        trip.ImagePath = "/images/" + ImagePath.FileName;
                    }
                    else if (string.IsNullOrEmpty(trip.ImagePath))
                    {
                        trip.ImagePath = "/images/default.jpg";
                    }

                    _context.Trips.Update(trip);
                    await _context.SaveChangesAsync();
                }
                catch (Exception)
                {
                    if (!_context.Trips.Any(t => t.Id == trip.Id))
                        return NotFound();
                    else
                        throw;
                }
                return RedirectToAction("AdminDashboard", "Account");
            }
            return View(trip);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            _context.Trips.Remove(trip);
            _context.SaveChanges();
            return RedirectToAction("AdminDashboard", "Account");
        }
    }
}
