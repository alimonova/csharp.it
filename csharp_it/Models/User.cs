﻿using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace csharp_it.Models
{
	public class User : IdentityUser<Guid>
    {
		public string? FirstName { get; set; }
		public string? LastName { get; set; }
        public string? RestorePasswordCode { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public Guid? RoleId { get; set; }
        public virtual Role Role { get; set; }
        public bool Blocked { get; set; }
        public string? TelegramId { get; set; }
        public string? Code { get; set; }
        public DateTime CodeGenerationDateTime { get; set; }
        public string? LastBotMessage { get; set; }
        public string? Avatar { get; set; }
        public string? Description { get; set; }
        public double WalletMoney { get; set; }
        public virtual Teacher Teacher { get; set; }

        public virtual List<UserTask> UserTasks { get; set; }
        public virtual List<UserCourse> UserCourses { get; set; }
        public virtual List<Solution> Solutions { get; set; }

		public User()
		{
            Blocked = false;

			UserTasks = new List<UserTask>();
            UserCourses = new List<UserCourse>();
            Solutions = new List<Solution>();
		}
	}
}

