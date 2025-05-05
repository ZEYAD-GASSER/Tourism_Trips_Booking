using Microsoft.AspNetCore.Mvc;
using Tourism_Trips_Booking.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;

namespace Tourism_Trips_Booking.Controllers
{
    [Authorize]
    public class TripController : Controller
    {
        private readonly Tourism_Trips_Booking_Context _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public TripController(Tourism_Trips_Booking_Context context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        private bool IsAdmin()
        {
            var role = HttpContext.Session.GetString("UserRole");
            return !string.IsNullOrEmpty(role) && role == "Admin";
        }

        private string SaveImage(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return "/images/default.jpg";

            var fileName = Path.GetFileNameWithoutExtension(file.FileName);
            var extension = Path.GetExtension(file.FileName);
            var newFileName = $"{fileName}_{Guid.NewGuid()}{extension}";
            var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images", newFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                file.CopyTo(stream);
            }

            return "/images/" + newFileName;
        }

        // GET: Trip/Create
        [HttpGet]
        public IActionResult Create()
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to create trips.";
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        // POST: Trip/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Trips trip, IFormFile ImagePath)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to create trips.";
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                trip.ImagePath = SaveImage(ImagePath);
                _context.Trips.Add(trip);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Trip created successfully.";
                return RedirectToAction("AdminDashboard", "Account");
            }
            return View(trip);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to edit trips.";
                return RedirectToAction("Index", "Home");
            }

            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
            {
                TempData["ErrorMessage"] = "Trip not found.";
                return RedirectToAction("AdminDashboard", "Account");
            }

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Trips trip, IFormFile ImagePath, string CurrentImagePath)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to edit trips.";
                return RedirectToAction("Index", "Home");
            }

            if (id != trip.Id)
            {
                TempData["ErrorMessage"] = "Invalid trip ID.";
                return RedirectToAction("AdminDashboard", "Account");
            }

            // Remove ImagePath from ModelState to prevent validation
            ModelState.Remove("ImagePath");

            if (ModelState.IsValid)
            {
                try
                {
                    var existingTrip = await _context.Trips.FindAsync(id);
                    if (existingTrip == null)
                    {
                        TempData["ErrorMessage"] = "Trip not found.";
                        return RedirectToAction("AdminDashboard", "Account");
                    }

                    // Update trip properties
                    existingTrip.Title = trip.Title;
                    existingTrip.Description = trip.Description;
                    existingTrip.Destination = trip.Destination;
                    existingTrip.HotelName = trip.HotelName;
                    existingTrip.TransportType = trip.TransportType;
                    existingTrip.StartDate = trip.StartDate;
                    existingTrip.EndDate = trip.EndDate;
                    existingTrip.price = trip.price;

                    // Handle image upload
                    if (ImagePath != null && ImagePath.Length > 0)
                    {
                        // Delete old image if it's not the default one
                        if (!string.IsNullOrEmpty(existingTrip.ImagePath) && existingTrip.ImagePath != "/images/default.jpg")
                        {
                            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, existingTrip.ImagePath.TrimStart('/'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        existingTrip.ImagePath = SaveImage(ImagePath);
                    }
                    else
                    {
                        // Keep the current image path
                        existingTrip.ImagePath = CurrentImagePath;
                    }

                    _context.Trips.Update(existingTrip);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "Trip updated successfully.";
                    return RedirectToAction("AdminDashboard", "Account");
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("", "An error occurred while updating the trip. Please try again.");
                    TempData["ErrorMessage"] = "An error occurred while updating the trip.";
                    return View(trip);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(trip);
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to delete trips.";
                return RedirectToAction("Index", "Home");
            }

            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            return View(trip);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            if (!IsAdmin())
            {
                TempData["ErrorMessage"] = "You do not have permission to delete trips.";
                return RedirectToAction("Index", "Home");
            }

            var trip = _context.Trips.FirstOrDefault(t => t.Id == id);
            if (trip == null)
                return NotFound();

            _context.Trips.Remove(trip);
            _context.SaveChanges();
            TempData["SuccessMessage"] = "Trip deleted successfully.";
            return RedirectToAction("AdminDashboard", "Account");
        }
    }
}
