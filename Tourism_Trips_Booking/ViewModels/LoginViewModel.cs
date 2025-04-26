using System.ComponentModel.DataAnnotations;

namespace Tourism_Trips_Booking.ViewModels
{
	public class LoginViewModel
	{
		[Required(ErrorMessage = "Email is required!")]
		[MaxLength(50, ErrorMessage = "Max 50 characters are allowed!")]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Password is required!")]
		[StringLength(20, MinimumLength = 8, ErrorMessage = "Max 20  or min 8 characters are allowed!")]
		[DataType(DataType.Password)]
		public required string Password { get; set; }
	}
}
