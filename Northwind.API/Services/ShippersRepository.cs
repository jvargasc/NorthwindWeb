using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class ShippersRepository : IShippersRepository
	{
		private NorthwindContext _context;

		public ShippersRepository(NorthwindContext context)
		{
			_context = context;
		}
		public IEnumerable<Shippers> GetShippers()
		{
			return _context.Shippers
					.OrderBy(c => c.ShipperId).ToList();
		}
	}
}
