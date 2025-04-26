using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class ReviewAndRating
    {
        [Key]
        public int Id { get; set; }

        public string? Comment { get; set; }

        public int? Rating { get; set; }

        [Required]
        [ForeignKey(nameof(Trips))]
        public int TripID { get; set; }

        public Trips Trips { get; set; }

        [Required]
        [ForeignKey(nameof(UserAccount))]
        public int UserID { get; set; }

        public UserAccount UserAccount { get; set; }
    }
}
