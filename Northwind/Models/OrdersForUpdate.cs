﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.Models
{
    public partial class OrdersForUpdate
    {
		[Required]
		public int OrderId { get; set; }
		[MaxLength(5)]
		public string CustomerId { get; set; }
        public int? EmployeeId { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? OrderDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
		public DateTime? RequiredDate { get; set; }
		[DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
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
	}
}
