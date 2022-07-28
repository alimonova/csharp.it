using System;
namespace csharp.it.Models
{
	public class UserGroup
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int GroupId { get; set; }
		public Group Group { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }

		public UserGroup()
        {
			Progress = 0;
        }
	}
}

