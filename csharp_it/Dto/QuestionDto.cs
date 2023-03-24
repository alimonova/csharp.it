namespace csharp_it.Dto
{
	public class QuestionDto
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Text { get; set; }
		public string Explanation { get; set; }
		public int LessonId { get; set; }
		public bool Multiple { get; set; }
	}
}

