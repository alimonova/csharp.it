using System;
namespace csharp_it.Dto
{
	public class CourseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid AuthorId { get; set; }
		public int TasksNum { get; set; }
		public int SecondsNum { get; set; }
		public double MinPrice { get; set; }
		public double MaxPrice { get; set; }
		public DateTime Created { get; set; }
		public int Duration { get; set; }
		public int LessonsNumber { get; set; }
	}
}

