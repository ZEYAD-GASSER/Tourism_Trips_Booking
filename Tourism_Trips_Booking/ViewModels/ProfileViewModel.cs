namespace Tourism_Trips_Booking.ViewModels
{
    public class ProfileViewModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string ProfilePicture { get; set; } // Path to the profile picture
        public string NewPassword { get; set; }
        public string ConfirmPassword { get; set; }
    }
}
