using Tourism_Trips_Booking.Models;

namespace Tourism_Trips_Booking.Services
{
    public class UserAccountService
    {
        private readonly Tourism_Trips_Booking_Context _context;

        public UserAccountService(Tourism_Trips_Booking_Context context)
        {
            _context = context;
        }

        public UserAccount? Login(string email, string password)
        {
            return _context.UserAccount.FirstOrDefault(u => u.Email == email && u.Password == password);
        }

        public void Register(UserAccount user)
        {
            _context.UserAccount.Add(user);
            _context.SaveChanges();
        }
    }
}
