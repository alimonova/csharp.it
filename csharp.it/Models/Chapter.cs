using System;
using System.Collections.Generic;

namespace csharp.it.Models
{
	public class Chapter
	{
		public int Id { get; set; }
		public int Number { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }

		public List<Lesson> Lessons { get; set; } 

		public Chapter()
		{
			Lessons = new List<Lesson>();
		}
	}
}

