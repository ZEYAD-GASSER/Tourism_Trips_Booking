using Microsoft.EntityFrameworkCore;
namespace Tourism_Trips_Booking.Models
{
    public class Tourism_Trips_Booking_Context : DbContext
    {
        public DbSet<UserAccount> UsersAccount { get; set; }
        public DbSet<Booking> Booking { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<Transport_Type> Transport_Type { get; set; }
        public DbSet<Trips> Trips { get; set; }
        public DbSet<ReviewAndRating> ReviewAndRating { get; set; }

    

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // connection string.
            optionsBuilder.UseSqlServer
               ("Data Source=LAPTOP-BRCLL69M\\SQLEXPRESS;Initial Catalog=Tourism_Trips_Booking;Integrated Security=True;Encrypt=False;Trust Server Certificate=True");
            base.OnConfiguring(optionsBuilder);
        }
    }
}
