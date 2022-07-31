using System;
namespace csharp_it.Models
{
	public class UserCourse
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int CourseId { get; set; }
		public virtual Course Course { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
		public Guid TarifId { get; set; }
		public virtual Tarif Tarif { get; set; }

		public UserCourse()
        {
			Progress = 0;
        }
	}
}

