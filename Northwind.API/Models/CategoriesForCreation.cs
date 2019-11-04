using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Models
{
    public partial class CategoriesForCreation
	{
		public int CategoryId { get; set; }
		[Required, MaxLength(30)]
        public string CategoryName { get; set; }
		[Required]
		public string Description { get; set; }
		[Required]
		public byte[] Picture { get; set; }

		//public ICollection<Products> Products { get; set; }
	}
}
