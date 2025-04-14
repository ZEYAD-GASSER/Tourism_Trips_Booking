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
        public decimal Price { get; set; }
        public bool? Manually_or_upon_receipt { get; set; }
        public bool? PayPal { get; set; }
    }
}
