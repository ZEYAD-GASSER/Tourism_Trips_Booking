using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Booking
    {
        public int Id { get; set; }
        public required string FromWhere { get; set; }
        public required string ToWhere { get; set; }
        public required DateOnly Depart { get; set; }
        public required DateOnly Return { get; set; }
        [ForeignKey("Trips")]
        public int TripID { get; set; }
        public required Trips Trips { get; set; }

        [ForeignKey("UserAccount")]
        public int UserID { get; set; }
        public required UserAccount UserAccount { get; set; }

    }
}
