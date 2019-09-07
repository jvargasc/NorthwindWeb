using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public class OrdersRepository : IOrdersRepository
	{
		private NorthwindContext _context;

		public OrdersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Orders>> GetOrders()
		{
			return await _context.Orders
					.Where( c => c.OrderId <= 10255)
					.OrderBy(c => c.OrderId).ToListAsync();
		}
		
		public async Task<Orders> GetOrder(int orderId)
		{
			return await _context.Orders
					.Where(c => c.OrderId == orderId).FirstOrDefaultAsync();
		}
	}
}
