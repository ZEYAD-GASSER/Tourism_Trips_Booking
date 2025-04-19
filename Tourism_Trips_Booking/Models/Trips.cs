using System.ComponentModel.DataAnnotations.Schema;

namespace Tourism_Trips_Booking.Models
{
    public class Trips
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Description { get; set; }
		public required string Destination { get; set; }


		public required string HotelName { get; set; }
		public required string TransportType { get; set; }

		public required DateTime StartDate { get; set; }
        public required DateTime EndDate { get; set; }
        public required double price { get; set; }


        public List<Booking>? Bookings { get; set; } 
        public List<ReviewAndRating>? Reviews { get; set; } 



    }
}
