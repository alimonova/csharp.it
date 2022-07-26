using System;
namespace csharp.it.Dto
{
	public class UserGroupDto
	{
		public int Id { get; set; }
		public double Progress { get; set; }
		public int GroupId { get; set; }
		public Guid UserId { get; set; }
	}
}

