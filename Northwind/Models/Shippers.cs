using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class Shippers
    {
		[Required]
		public int ShipperId { get; set; }
		[Required, MaxLength(40)]
		public string CompanyName { get; set; }
		[MaxLength(24)]
		public string Phone { get; set; }

    }
}
