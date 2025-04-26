using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class ReviewAndRating
    {
       
            public int Id{ get; set; }
            public string? Comment { get; set; }
            public int? Rating { get; set; } 


        [ForeignKey("Trips")]
            public int TripID { get; set; }
            required public Trips Trips { get; set; }

            [ForeignKey("UserAccount")]
            public int UserID { get; set; }
            required public UserAccount UserAccount { get; set; }

    }
}
