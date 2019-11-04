﻿namespace Northwind.Models
{
	public class OrderDetails
	{
		public int OrderId { get; set; }
		public int ProductId { get; set; }
		public decimal UnitPrice { get; set; }
		public short Quantity { get; set; }
		public float Discount { get; set; }
		public Products Products { get; set; }

	}
}