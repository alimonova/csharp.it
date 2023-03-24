using System;
namespace csharp_it.Dto
{
	public class UserCourseDto
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int GroupId { get; set; }
		public Guid UserId { get; set; }
		public CourseDto Course { get; set; }
		public DateTime Expiration { get; set; }
		public int CurrentLessonNumber { get; set; } = 1;
		public bool Paid { get; set; } = false;
	}
}

