using System;
using csharp_it.Dto;

namespace csharp_it.Models
{
	public class NewTarif
	{
		public NewTarifDto Tarif { get; set; }
		public int[] Accesses { get; set; }
	}
}

