using Northwind.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public class CustomersRepository : ICustomersRepository
	{
		private NorthwindContext _context;

		public CustomersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Customers> GetCustomers()
		{
			return _context.Customers
					.OrderBy(c => c.CustomerId).ToList();
		}

	}
}
