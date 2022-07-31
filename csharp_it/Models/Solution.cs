using System;
namespace csharp_it.Models
{
	public class Solution
	{
		public Guid Id { get; set; }
		public Guid UserId { get; set; }
		public User User { get; set; }
		public int TaskId { get; set; }
		public Task Task { get; set; }
		public string? Link { get; set; }
		public int AttemptNumber { get; set; }
		public double Mark { get; set; }
		public string Comment { get; set; }
	}
}

