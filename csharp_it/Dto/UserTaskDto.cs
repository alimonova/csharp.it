using System;
namespace csharp_it.Dto
{
	public class UserTaskDto
	{
		public int Id { get; set; }
		public Guid StudentId { get; set; }
		public Guid TeacherId { get; set; }
		public string? Comment { get; set; }
		public double? Mark { get; set; }
		public int Status { get; set; }
		public int TaskId { get; set; }
		public string? Link { get; set; }
	}
}

