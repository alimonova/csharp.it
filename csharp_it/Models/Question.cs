using System.Collections.Generic;

namespace csharp_it.Models
{
	public class Question
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Text { get; set; }
		public string Explanation { get; set; }
		public int LessonId { get; set; }
		public virtual Lesson Lesson { get; set; }

		public virtual List<Answer> Answers { get; set; }

		public Question()
		{
			Answers = new List<Answer>();
		}
	}
}

