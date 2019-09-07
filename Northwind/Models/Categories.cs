using System;
using System.Collections.Generic;
using System.IO;

namespace Northwind.Models
{
	public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

		public int Id { get; set; }
		public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
		public string Picture64 { get; set; }
		public ICollection<Products> Products { get; set; }
	}
}
