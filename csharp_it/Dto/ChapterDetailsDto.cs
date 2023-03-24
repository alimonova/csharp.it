using System;
namespace csharp_it.Dto
{
	public class ChapterDetailsDto
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public int CourseId { get; set; }
		public int SecondsNum { get; set; }
		public List<LessonDetailsDto> Lessons { get; set; }
	}
}

