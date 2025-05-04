using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class ReviewAndRating
    {
        [Key]
        public int Id { get; set; }

        [MaxLength(1000)]
        public string? Comment { get; set; }

        [Range(1, 5)]
        public int? Rating { get; set; }

        [Required]
        [ForeignKey(nameof(Trips))]
        public int TripID { get; set; }

        [Required]
        public Trips Trips { get; set; }

        [Required]
        [ForeignKey(nameof(UserAccount))]
        public int UserID { get; set; }

        [Required]
        public UserAccount UserAccount { get; set; }
    }

}
