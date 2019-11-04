using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
	public partial class ProductsForCreation
    {

		public int ProductId { get; set; }
		[MaxLength(40)]
		public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
		[MaxLength(20)]
		public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

		public Categories Categories { get; set; }
		public Suppliers Suppliers { get; set; }
	}
}
