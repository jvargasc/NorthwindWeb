using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class Categories
    {
        public Categories()
        {
            Products = new HashSet<Products>();
        }

		public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string Description { get; set; }
		public byte[] Picture { get { return _picture; }
			                    set { _picture = value; } }
		private byte[] _picture { get; set; }
		public string Picture64
		{
			get { return Utilities.Base64String(_picture); }
		}

		public ICollection<Products> Products { get; set; }
    }
}
