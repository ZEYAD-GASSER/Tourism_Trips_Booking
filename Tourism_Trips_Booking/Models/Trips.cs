using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Trips
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
        public required DateOnly StartDate { get; set; }
        public required DateOnly EndDate { get; set; }
        public required double price { get; set; }
        [ForeignKey("TransportType")]
        public required int TranspirtTypeID {  get; set; }
        public required Transport_Type TransportType { get; set; }

        //Navigation Property
        public List<Booking>? Bookings { get; set; } 
        public List<ReviewAndRating>? Reviews { get; set; } 



    }
}
