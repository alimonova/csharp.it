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
		public int TeacherId { get; set; }
		public virtual Teacher Teacher { get; set; }
		public int TasksNum { get; set; } = 0;
		public int SecondsNum { get; set; } = 0;
		public double MinPrice { get; set; } = 0;
		public double MaxPrice { get; set; } = 0;
		public int Duration { get; set; }

		public DateTime Created { get; set; }

		public virtual List<Chapter> Chapters { get; set; }
		public virtual List<Tarif> Tarifs { get; set; }

		public Course()
		{
			Chapters = new List<Chapter>();
			Tarifs = new List<Tarif>();
		}
	}
}

