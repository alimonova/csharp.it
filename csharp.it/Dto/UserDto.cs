using System;
namespace csharp.it.Dto
{
	public class UserDto
	{
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string RestorePasswordCode { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Guid? RoleId { get; set; }
        public bool Blocked { get; set; }
        public string TelegramId { get; set; }
        public string Code { get; set; }
        public DateTime CodeGenerationDateTime { get; set; }
        public string LastBotMessage { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }
    }
}

