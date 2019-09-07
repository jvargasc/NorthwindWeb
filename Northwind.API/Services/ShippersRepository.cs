using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public class ShippersRepository : IShippersRepository
	{
		private NorthwindContext _context;

		public ShippersRepository(NorthwindContext context)
		{
			_context = context;
		}
		public async Task<IEnumerable<Shippers>> GetShippers()
		{
			return await _context.Shippers
					.OrderBy(c => c.ShipperId).ToListAsync();
		}

		public async Task<Shippers> GetShipper(int shipperId)
		{
			return await _context.Shippers
					.Where(c => c.ShipperId == shipperId).FirstOrDefaultAsync();
		}
	}
}
