using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;
using System;

namespace Northwind.API.Services
{
	public class ShippersRepository : IShippersRepository, IDisposable
	{
		private NorthwindContext _context;

		public ShippersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Shippers.CountAsync();
		}

		public async Task<IEnumerable<Shippers>> GetShippers(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Shippers
					.OrderBy(c => c.ShipperId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Shippers.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Shippers
					.OrderBy(c => c.ShipperId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Shippers> GetShipper(int shipperId)
		{
			return await _context.Shippers
					.Where(c => c.ShipperId == shipperId).FirstOrDefaultAsync();
		}

		public async void AddShipper(Shippers shipperToAdd)
		{
			if (shipperToAdd == null)
			{
				throw new ArgumentNullException(nameof(shipperToAdd));
			}

			await _context.AddAsync(shipperToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> ShipperExits(int shipperId)
		{
			return (await _context.Shippers.AnyAsync(e => e.ShipperId == shipperId));
		}

		public void DeleteShipper(Shippers shipperToDelete)
		{
			if (shipperToDelete == null)
			{
				throw new ArgumentNullException(nameof(shipperToDelete));
			}

			_context.Remove(shipperToDelete);
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
