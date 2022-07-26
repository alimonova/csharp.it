using System.Collections.Generic;

namespace csharp.it.Models
{
	public class Lesson
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public int ChapterId { get; set; }
		public Chapter Chapter { get; set; }
		public string Topic { get; set; }
		public string ContentLink { get; set; }
		public string VideoLink { get; set; }

		public List<Question> Questions { get; set; }
		public List<PracticalExample> PracticalExamples { get; set; }
		public List<UsefulResource> UsefulResources { get; set; }
		public List<Task> Tasks { get; set; }

		public Lesson()
		{
			Questions = new List<Question>();
			PracticalExamples = new List<PracticalExample>();
			UsefulResources = new List<UsefulResource>();
			Tasks = new List<Task>();
		}
	}
}

