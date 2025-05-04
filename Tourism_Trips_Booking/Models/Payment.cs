using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Payment
    {
        [Key]
        [ForeignKey(nameof(Booking))]
        public int BookingID { get; set; }

        [Required]
        public Booking Booking { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public double Price { get; set; }

        [Required]
        [MaxLength(20)]
        public string CashOrCard { get; set; }
    }

}
