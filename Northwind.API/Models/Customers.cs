using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class Customers
    {
		//public Customers()
		//{
		//    CustomerCustomerDemo = new HashSet<CustomerCustomerDemo>();
		//    Orders = new HashSet<Orders>();
		//}

		//public Customers()
		//{
		//	Region = new Region();
		//}

		public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
		public int RegionId { get; set; }
		public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

		public Regions Regions { get; set; }

		//public Region RegionDescription { get; set; }
		//public ICollection<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
		//public ICollection<Orders> Orders { get; set; }
	}
}
