using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class TerritoriesForUpdate
	{
		[Required, MaxLength(20)]
		public string TerritoryId { get; set; }
		[MaxLength(50)]
		public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

		public Regions Regions { get; set; }
	}
}
