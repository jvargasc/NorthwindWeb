﻿using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class ProductsForUpdate
    {
        //public Products()
        //{
        //    OrderDetails = new HashSet<OrderDetails>();
        //}

        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public int? SupplierId { get; set; }
        public int? CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal? UnitPrice { get; set; }
        public short? UnitsInStock { get; set; }
        public short? UnitsOnOrder { get; set; }
        public short? ReorderLevel { get; set; }
        public bool Discontinued { get; set; }

		public Categories Categories { get; set; }
		public Suppliers Suppliers { get; set; }
		//public ICollection<OrderDetails> OrderDetails { get; set; }
	}
}