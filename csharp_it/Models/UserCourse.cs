using System;
namespace csharp_it.Models
{
	public class UserCourse
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public Guid TarifId { get; set; }
		public Tarif Tarif { get; set; }

		public UserCourse()
        {
			Progress = 0;
        }
	}
}

