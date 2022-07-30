using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace csharp_it.Models
{
	public class Course
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public Guid AuthorId { get; set; }
		[JsonIgnore]
		public User Author { get; set; }
		public int TasksNum { get; set; }
		public int SecondsNum { get; set; }

		public List<Chapter> Chapters { get; set; }
		public List<Tarif> Tarifs { get; set; }
		public List<UserCourse> Students { get; set; }

		public Course()
		{
			TasksNum = 0;
			SecondsNum = 0;

			Chapters = new List<Chapter>();
			Tarifs = new List<Tarif>();
			Students = new List<UserCourse>();
		}
	}
}

