using System;
namespace csharp_it.Dto
{
	public class LessonDetailsDto
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int ChapterId { get; set; }
		public string Topic { get; set; }
		public string ContentLink { get; set; }
		public string VideoLink { get; set; }
		public string Description { get; set; }
		public List<QuestionDetailsDto> Questions { get; set; }
		public List<TaskDto> Tasks { get; set; }
		public List<PracticalExampleDto> PracticalExamples { get; set; }
		public List<UsefulResourceDto> UsefulResources { get; set; }
	}
}

