using System;
using System.ComponentModel.DataAnnotations;

namespace csharp_it.Models
{
	public class ResetPassword
	{
        [Required]
        public string Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password should contain at least 6 symbols", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public string Code { get; set; }
    }
}

