using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class Categories
    {

		[Key]
		public int CategoryId { get; set; }
		[Required, MaxLength(15)]
		public string CategoryName { get; set; }
		public string Description { get; set; }
		public byte[] Picture { get; set; }

    }
}
