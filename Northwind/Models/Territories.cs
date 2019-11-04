using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class Territories
    {
		[Required, MaxLength(20)]
		public string TerritoryId { get; set; }
		[Required, MaxLength(50)]
		public string TerritoryDescription { get { return _territoryDescription; }
											 set { _territoryDescription = value.Trim(); } }
        public int RegionId { get; set; }
		private string _territoryDescription;

		public Regions Regions { get; set; }
	}
}
