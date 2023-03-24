using System;
namespace csharp_it.Dto
{
	public class TarifDetailsDto
	{
		public Guid Id { get; set; }
		public string Name { get; set; }
		public int CourseId { get; set; }
		public double Price { get; set; }
		public string Description { get; set; }
		public double PriceMonth { get; set; }
		public double PriceYear { get; set; }
		public bool Popular { get; set; }
		public IEnumerable<TarifAccessDto> TarifAccesses { get; set; }
	}
}

