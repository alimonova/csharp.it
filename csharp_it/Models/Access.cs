using System;
namespace csharp_it.Models
{
	public class Access
	{
		public int Id { get; set; }
		public string Name { get; set; }
		public string? Description { get; set; }

		public List<TarifAccess> TarifAccesses { get; set; }

		public Access()
        {
			TarifAccesses = new List<TarifAccess>();
        }
	}
}

