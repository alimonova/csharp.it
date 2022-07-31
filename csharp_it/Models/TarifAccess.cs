using System;
namespace csharp_it.Models
{
	public class TarifAccess
	{
		public int Id { get; set; }
		public Guid TarifId { get; set; }
		public virtual Tarif Tarif { get; set; }
		public int AccessId { get; set; }
		public virtual Access Access { get; set; }
	}
}

