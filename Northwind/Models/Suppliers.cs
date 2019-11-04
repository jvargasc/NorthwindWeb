using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class Suppliers
    {
		[Required]
		public int SupplierId { get; set; }
		[Required, MaxLength(40)]
        public string CompanyName { get; set; }
		[MaxLength(30)]
		public string ContactName { get; set; }
		[MaxLength(30)]
		public string ContactTitle { get; set; }
		[MaxLength(60)]
		public string Address { get; set; }
		[MaxLength(15)]
		public string City { get; set; }
		public int RegionId { get; set; }
		[MaxLength(10)]
		public string PostalCode { get; set; }
		[MaxLength(15)]
		public string Country { get; set; }
		[MaxLength(24)]
		public string Phone { get; set; }
		[MaxLength(24)]
		public string Fax { get; set; }
        public string HomePage { get; set; }

		public Regions Regions { get; set; }
	}
}
