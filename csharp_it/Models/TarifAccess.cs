using System;
namespace csharp_it.Models
{
	public class TarifAccess
	{
		public int Id { get; set; }
		public Guid TarifId { get; set; }
		public Tarif Tarif { get; set; }
		public int AccessId { get; set; }
		public Access Access { get; set; }
	}
}

