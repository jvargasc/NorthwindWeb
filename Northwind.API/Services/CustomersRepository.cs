using Northwind.API.Models;
using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<Customers>> GetCustomers()
		{
			return await _context.Customers
					.OrderBy(c => c.CustomerId).ToListAsync();
		}

		public async Task<Customers> GetCustomer(string customerId)
		{
			return await _context.Customers
					.Where(c => c.CustomerId == customerId)
					.FirstOrDefaultAsync();
		}

	}
}
