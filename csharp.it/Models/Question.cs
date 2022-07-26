using System.Collections.Generic;

namespace csharp.it.Models
{
	public class Question
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Text { get; set; }
		public string Explanation { get; set; }
		public int RightAnswerId { get; set; }
		public Answer RightAnswer { get; set; }
		public int LessonId { get; set; }
		public Lesson Lesson { get; set; }

		public List<Answer> Answers { get; set; }

		public Question()
		{
			Answers = new List<Answer>();
		}
	}
}

