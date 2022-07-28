using System;
namespace csharp.it.Models
{
	public class UserCourse
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }

		public UserCourse()
        {
			Progress = 0;
        }
	}
}

