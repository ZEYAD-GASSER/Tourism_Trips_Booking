using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Payment
    {
        [Key]
        [ForeignKey("Booking")]//because it one to one relationship  the id of Payment is ForeignKey from booking
        public required int BookingID { get; set; }
        public required Booking Booking { get; set; }
        public double Price { get; set; }
        public required string CashOrCard { get; set; }

    }
}
