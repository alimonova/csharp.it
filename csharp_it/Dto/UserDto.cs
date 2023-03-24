using System;
namespace csharp_it.Dto
{
	public class UserDto
	{
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Blocked { get; set; }
        public string TelegramId { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

