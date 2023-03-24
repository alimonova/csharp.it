using System.Collections.Generic;

namespace csharp_it.Models
{
	public class Lesson
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int ChapterId { get; set; }
		public virtual Chapter Chapter { get; set; }
		public string Topic { get; set; }
		public string? Description { get; set; }
		public string? ContentLink { get; set; }
		public string? VideoLink { get; set; }

		public virtual List<Question> Questions { get; set; }
		public virtual List<PracticalExample> PracticalExamples { get; set; }
		public virtual List<UsefulResource> UsefulResources { get; set; }
		public virtual List<Task> Tasks { get; set; }

		public Lesson()
		{
			Questions = new List<Question>();
			PracticalExamples = new List<PracticalExample>();
			UsefulResources = new List<UsefulResource>();
			Tasks = new List<Task>();
		}
	}
}

