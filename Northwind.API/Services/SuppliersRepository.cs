using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;
using System;

namespace Northwind.API.Services
{
	public class SuppliersRepository : ISuppliersRepository, IDisposable
	{
		private NorthwindContext _context;

		public SuppliersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Suppliers.CountAsync();
		}

		public async Task<IEnumerable<Suppliers>> GetSuppliers(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Suppliers
								.Include(r => r.Regions)
								.OrderBy(c => c.SupplierId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Suppliers.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Suppliers
								.Include(r => r.Regions)
								.OrderBy(c => c.SupplierId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Suppliers> GetSupplier(int supplierId)
		{
			return await _context.Suppliers
						.Include(r => r.Regions)
						.Where(c => c.SupplierId == supplierId).FirstOrDefaultAsync();
		}

		public async void AddSupplier(Suppliers supplierToAdd)
		{
			if (supplierToAdd == null)
			{
				throw new ArgumentNullException(nameof(supplierToAdd));
			}

			await _context.AddAsync(supplierToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> SupplierExits(int supplierId)
		{
			return (await _context.Suppliers.AnyAsync(e => e.SupplierId == supplierId));
		}

		public void DeleteSupplier(Suppliers supplierToDelete)
		{
			if (supplierToDelete == null)
			{
				throw new ArgumentNullException(nameof(supplierToDelete));
			}

			_context.Remove(supplierToDelete);
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
