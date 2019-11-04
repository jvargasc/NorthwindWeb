﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Models
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
		}

		public int OrderId { get; set; }
        public string CustomerId { get; set; }
		public int? EmployeeId { get; set; }
        public DateTime? OrderDate { get; set; }
        public DateTime? RequiredDate { get; set; }
        public DateTime? ShippedDate { get; set; }
        public int? ShipperId { get; set; }
        public decimal? Freight { get; set; }
        public string ShipName { get; set; }
        public string ShipAddress { get; set; }
        public string ShipCity { get; set; }
		public int RegionId { get; set; }
		public string ShipPostalCode { get; set; }
        public string ShipCountry { get; set; }

		public Customers Customers { get; set; }
		public Employees Employees { get; set; }
		public Regions Regions { get; set; }
		public Shippers Shippers { get; set; }
		public ICollection<OrderDetails> OrderDetails { get; set; }
	}
}
