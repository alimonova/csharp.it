using System;
namespace csharp_it.Dto
{
	public class SolutionCreationDto
	{
		public Guid UserId { get; set; }
		public int TaskId { get; set; }
		public string? Link { get; set; }
	}
}

