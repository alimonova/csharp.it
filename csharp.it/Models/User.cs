using System;
using System.Collections.Generic;

namespace csharp.it.Models
{
	public class User
	{
		public string FirstName { get; set; }
		public string LastName { get; set; }
        public string RestorePasswordCode { get; set; }
        public string PictureUrl { get; set; }
        public string Password { get; set; }
        public string Token { get; set; }
        public Guid? RoleId { get; set; }
        public Role Role { get; set; }
        public bool Blocked { get; set; }
        public string TelegramId { get; set; }
        public string Code { get; set; }
        public DateTime CodeGenerationDateTime { get; set; }
        public string LastBotMessage { get; set; }
        public string Avatar { get; set; }
        public string Description { get; set; }

        public List<UserTask> UserTasks { get; set; }

		public User()
		{
			UserTasks = new List<UserTask>();
		}
	}
}

