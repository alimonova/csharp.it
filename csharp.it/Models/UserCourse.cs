using System;
namespace csharp.it.Models
{
	public class UserCourse
	{
		public double Progress { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }

		public UserCourse()
		{
		}
	}
}

