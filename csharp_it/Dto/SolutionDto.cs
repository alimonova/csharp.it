using System;
namespace csharp_it.Dto
{
	public class SolutionDto
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public int TaskId { get; set; }
		public string? Link { get; set; }
		public int AttemptNumber { get; set; }
		public double Mark { get; set; }
		public string Comment { get; set; }
	}
}

