using System;
using System.ComponentModel.DataAnnotations;

namespace csharp_it.Models
{
	public class ForgotPassword
	{
		[Required]
		[EmailAddress]
		public string Email { get; set; }
	}
}

