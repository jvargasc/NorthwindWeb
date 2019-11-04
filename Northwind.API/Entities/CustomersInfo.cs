using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class CustomersInfo
	{
		[Key]
		public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }

    }
}
