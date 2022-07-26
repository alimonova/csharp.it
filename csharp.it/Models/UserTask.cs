using System;
namespace csharp.it.Models
{
	public class UserTask
	{
		public int Id { get; set; }
		public Guid StudentId { get; set; }
		public User Student { get; set; }
		public Guid TeacherId { get; set; }
		public User Teacher { get; set; }
		public string Comment { get; set; }
		public double? Mark { get; set; }
		// 0 - not checked
        // 1 - checked and ok
        // -1 - checked but not ok
		public int Status { get; set; }
		public int TaskId { get; set; }
		public Task Task { get; set; }
		public string Link { get; set; }
		
		public UserTask()
        {
			Status = 0;
        }
	}
}

