using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class TerritoriesForCreation
    {
		[MaxLength(20)]
		public string TerritoryId { get; set; }
		[Required, MaxLength(50)]
		public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

		public Regions Regions { get; set; }

	}
}
