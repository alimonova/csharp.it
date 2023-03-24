using System;
namespace csharp_it.Models
{
	public class Teacher
	{
		public int Id { get; set; }
		public Guid UserId { get; set; }
		public virtual User User { get; set; }
		public string Description { get; set; }

		public virtual List<Course> Courses { get; set; }

		public Teacher()
        {
			Courses = new List<Course>();
        }
	}	
}

