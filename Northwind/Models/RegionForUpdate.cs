using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class RegionForUpdate
    {

		[Required]
		public int RegionId { get; set; }
		[Required, MaxLength(50)]
		public string RegionDescription { get; set; }

    }
}
