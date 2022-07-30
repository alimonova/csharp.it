using System;
namespace csharp_it.Dto
{
	public class TarifDto
	{
		public Guid Id { get; set; }
		public int CourseId { get; set; }
		public double Price { get; set; }
		public int Currency { get; set; }
		public string Description { get; set; }
		public string Access { get; set; }
	}
}

