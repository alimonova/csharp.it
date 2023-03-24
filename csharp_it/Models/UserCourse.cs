using System;
namespace csharp_it.Models
{
	public class UserCourse
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
		public Guid TarifId { get; set; }
		public virtual Tarif Tarif { get; set; }
		public DateTime Expiration { get; set; }
		public int CurrentLessonNumber { get; set; } = 1;
		public bool Paid { get; set; } = false;

		public UserCourse()
        {
			Progress = 0;
        }
	}
}

