﻿using System.ComponentModel.DataAnnotations;

namespace Tourism_Trips_Booking.ViewModels
{
	public class RegisterationViewModels
	{
		[Required(ErrorMessage = "Name is required!")]
		[MaxLength(50, ErrorMessage = "Max 50 characters are allowed!")]
		public required string Name { get; set; }

		[Required(ErrorMessage = "Email is required!")]
		[MaxLength(50, ErrorMessage = "Max 50 characters are allowed!")]
		[EmailAddress]
		public required string Email { get; set; }

		[Required(ErrorMessage = "Password is required!")]
		[StringLength(20,MinimumLength = 8, ErrorMessage = "Max 20  or min 8 characters are allowed!")]
		[DataType(DataType.Password)]
		public required string Password { get; set; }

		[Compare("Password", ErrorMessage = "Please confirm your password!")]
		[DataType(DataType.Password)]
		public required string ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Profile picture is required!")]
        public required IFormFile ProfilePicture { get; set; }
    }
}
