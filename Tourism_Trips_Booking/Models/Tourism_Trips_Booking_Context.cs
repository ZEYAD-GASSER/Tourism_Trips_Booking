using Microsoft.EntityFrameworkCore;

namespace Tourism_Trips_Booking.Models
{
    public class Tourism_Trips_Booking_Context : DbContext
    {
        public Tourism_Trips_Booking_Context(DbContextOptions<Tourism_Trips_Booking_Context> options)
            : base(options)
        {
        }

        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Trips> Trips { get; set; }
        public DbSet<ReviewAndRating> ReviewAndRating { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                "Server = M_YAHIA\\M_YAHIA;Database = Tourism_Trips_Booking;Integrated Security= SSPI; Trust Server Certificate=True;");
        }

    }
}
