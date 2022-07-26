using System;
namespace csharp.it.Dto
{
	public class CourseDto
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid AuthorId { get; set; }
		public int TasksNum { get; set; }
		public int SecondsNum { get; set; }
	}
}

