using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class OrdersRepository : IOrdersRepository
	{
		private NorthwindContext _context;

		public OrdersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Orders> GetOrders()
		{
			return _context.Orders
					.OrderBy(c => c.OrderId).ToList();
		}
	}
}
