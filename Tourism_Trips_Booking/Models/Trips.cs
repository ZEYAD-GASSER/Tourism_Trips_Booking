using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Trips
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Destination { get; set; }

        [Required]
        public string HotelName { get; set; }

        [Required]
        public string TransportType { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public double price { get; set; }

        public string? ImagePath { get; set; }

        public List<Booking>? Bookings { get; set; }
        public List<ReviewAndRating>? Reviews { get; set; }
    }
}
