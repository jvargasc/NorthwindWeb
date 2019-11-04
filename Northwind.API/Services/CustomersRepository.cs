using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public class CustomersRepository : ICustomersRepository, IDisposable
	{
		private NorthwindContext _context;

		public CustomersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Customers.CountAsync();
		}

		public async Task<IEnumerable<Customers>> GetCustomers(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Customers
					.Include(r => r.Regions)
					.OrderBy(c => c.CustomerId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Customers.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Customers
						.Include(r => r.Regions)
						.OrderBy(c => c.CustomerId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Customers> GetCustomer(string customerId)
		{
			return await _context.Customers
					.Where(c => c.CustomerId == customerId)
					.Include(r => r.Regions)
					.FirstOrDefaultAsync();
		}

		public async void AddCustomer(Customers customerToAdd)
		{
			if (customerToAdd == null)
			{
				throw new ArgumentException(nameof(customerToAdd));
			}

			await _context.AddAsync(customerToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);		
		}

		public async Task<bool> CustomerExits(string customerId)
		{
			return (await _context.Customers.AnyAsync(e => e.CustomerId == customerId));
		}

		public void DeleteCustomer(Customers customerToDelete)
		{
			if (customerToDelete == null)
			{
				throw new ArgumentNullException(nameof(customerToDelete));
			}

			_context.Remove(customerToDelete);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}
	}
}
