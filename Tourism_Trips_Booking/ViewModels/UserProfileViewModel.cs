using System.ComponentModel.DataAnnotations;

namespace Tourism_Trips_Booking.ViewModels
{
    public class UserProfileViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        public string? ExistingProfilePicture { get; set; } // Made nullable

        public IFormFile? ProfilePicture { get; set; } // Made nullable
        public int BookingCount { get; set; } // For the number of bookings
        public int PreviousTripsCount { get; set; }

        // For password reset - made optional
        [DataType(DataType.Password)]
        public string? NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string? ConfirmNewPassword { get; set; }
    }
}
