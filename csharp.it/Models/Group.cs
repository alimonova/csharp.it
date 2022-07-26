using System;
using System.Collections.Generic;

namespace csharp.it.Models
{
	public class Group
	{
		public int Id { get; set; }
		public Guid TeacherId { get; set; }
		public User Teacher { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }

		public List<UserCourse> Students { get; set; }

		public Group()
		{
			Students = new List<UserCourse>();
		}
	}
}

