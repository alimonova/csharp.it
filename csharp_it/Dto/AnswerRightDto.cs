using System;
namespace csharp_it.Dto
{
	public class AnswerRightDto
	{
		public int Id { get; set; }
		public int QuestionId { get; set; }
		public string? Text { get; set; }
		public bool RightAnswer { get; set; }
	}
}

