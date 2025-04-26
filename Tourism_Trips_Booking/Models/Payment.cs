using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Payment
    {
        [Key]
        [ForeignKey(nameof(Booking))]
        public int BookingID { get; set; }

        public Booking Booking { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string CashOrCard { get; set; }
    }
}
