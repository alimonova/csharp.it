using System;
namespace csharp.it.Models
{
	public class Tarif
	{
		public Guid Id { get; set; }
		public int CourseId { get; set; }
		public Course Course { get; set; }
		public double Price { get; set; }
		public int Currency { get; set; }
		public string Description { get; set; }
		public string Access { get; set; }

		public Tarif()
		{
		}
	}
}

