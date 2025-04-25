using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Booking
    {
        public int Id { get; set; }

        [ForeignKey("Trips")]
        public int TripID { get; set; }
        public required Trips Trips { get; set; }

        [ForeignKey("UserAccount")]
        public int UserID { get; set; }
        public required UserAccount UserAccount { get; set; }


        public DateTime BookingDate { get; set; }
    }
}
