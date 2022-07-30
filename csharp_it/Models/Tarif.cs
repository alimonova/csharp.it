using System;
using System.Collections.Generic;

namespace csharp_it.Models
{
	public class Tarif
	{
		public Guid Id { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }
		public double Price { get; set; }
		// 0 - USD
		// 1 - UAH
		// 2 - EUR
		public int Currency { get; set; }
		public string Description { get; set; }
		public string Access { get; set; }

		public List<UserCourse> UserCourses { get; set; }

		public Tarif()
        {
			UserCourses = new List<UserCourse>();
        }
	}
}

