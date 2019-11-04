using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class CustomersForCreation
    {
        //public Customers()
        //{
        //    CustomerCustomerDemo = new HashSet<CustomerCustomerDemo>();
        //    Orders = new HashSet<Orders>();
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

		public void GenerateCustomerId()
		{
			int spaceIndex = CompanyName.IndexOf(' ');
			string tmpCustomerId = string.Empty;

			if (spaceIndex == 0)
				tmpCustomerId = CompanyName.ToUpper().Substring(0, 5);
			else
				tmpCustomerId = CompanyName.ToUpper().Substring(0, 3) +
								CompanyName.ToUpper().Substring(spaceIndex + 1, 2);

			CustomerId = tmpCustomerId;
		}

		public Regions Regions { get; set; }
		//public ICollection<CustomerCustomerDemo> CustomerCustomerDemo { get; set; }
		//public ICollection<Orders> Orders { get; set; }
	}
}
