using System;
using csharp_it.Models;

namespace csharp_it.Dto
{
	public class CourseDetailsDto
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
		public bool CheckTest { get; set; } = true;
		public bool SendTask { get; set; } = true;
		public List<ChapterDetailsDto> Chapters { get; set; }
		public List<TarifDetailsDto> Tarifs { get; set; }
	}
}

