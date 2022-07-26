namespace csharp.it.Models
{
	public class Answer
	{
		public int Id { get; set; }
		public int QuestionId { get; set; }
		public Question Question { get; set; }
		public string Text { get; set; }
	}
}

