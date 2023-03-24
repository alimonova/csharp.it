using System;
using System.Collections.Generic;

namespace csharp_it.Models
{
	public class Tarif
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int CourseId { get; set; }
		public virtual Course Course { get; set; }
		public double PriceMonth { get; set; }
		public double PriceYear { get; set; }
		public double FullPrice { get; set; }
		public bool OnceBilling { get; set; } = false;
		public string Description { get; set; }
		public bool Popular { get; set; }

		public virtual List<UserCourse> UserCourses { get; set; }
		public virtual List<TarifAccess> TarifAccesses { get; set; }

		public Tarif()
        {
			UserCourses = new List<UserCourse>();
			TarifAccesses = new List<TarifAccess>();
        }
	}
}

