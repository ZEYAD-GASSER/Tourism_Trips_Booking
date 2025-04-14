using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; } //Admin or User

        //Navigation Property
        public List<Booking>? Bookings { get; set; }

    }
}
