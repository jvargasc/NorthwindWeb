using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class Shippers
    {
		[Key]
        public int ShipperId { get; set; }
		[Required, MaxLength(40)]
        public string CompanyName { get; set; }
		[MaxLength(24)]
		public string Phone { get; set; }

    }
}
