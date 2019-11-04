using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class Orders
    {
		public Orders()
		{
			Customers = new Customers();
			Employees = new Employees();
			Regions = new Regions();
			Shippers = new Shippers();
			OrderDetails = new HashSet<OrderDetails>();
			//Products = new Products();
		}

		[Key]
        public int OrderId { get; set; }
		[MaxLength(5)]
		public string CustomerId { get; set; }
		public int? EmployeeId { get; set; }
		public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipperId { get; set; }
        public decimal? Freight { get; set; }
		[MaxLength(40)]
		public string ShipName { get; set; }
		[MaxLength(60)]
		public string ShipAddress { get; set; }
		[MaxLength(15)]
		public string ShipCity { get; set; }
		public int RegionId { get; set; }
		[MaxLength(10)]
		public string ShipPostalCode { get; set; }
		[MaxLength(15)]
		public string ShipCountry { get; set; }

		public Customers Customers { get; set; }
		public Employees Employees { get; set; }
		public Regions Regions { get; set; }
		public Shippers Shippers { get; set; }
		public ICollection<OrderDetails> OrderDetails { get; set; }
		//public Products Products { get; set; }
	}
}
