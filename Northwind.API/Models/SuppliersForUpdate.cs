using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class SuppliersForUpdate
    {
        //public Suppliers()
        //{
        //    Products = new HashSet<Products>();
        //}

        public int SupplierId { get; set; }
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
        public string HomePage { get; set; }

		public Regions Regions { get; set; }

		//public ICollection<Products> Products { get; set; }
	}
}
