using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Booking
    {
        [Key]
        public int Id { get; set; }

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
