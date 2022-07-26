using System;
namespace csharp.it.Models
{
	public class UsefulResource
	{
		public int Id { get; set; }
		public string Link { get; set; }
		public string Description { get; set; }
		public bool Advanced { get; set; }
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public UsefulResource()
		{
		}
	}
}

