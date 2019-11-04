using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class ShippersForCreation
    {
		public int ShipperId { get; set; }
		[Required, MaxLength(40)]
		public string CompanyName { get; set; }
		[MaxLength(20)]
		public string Phone { get; set; }

    }
}
