using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;
using System;

namespace Northwind.API.Services
{
	public class ProductsRepository : IProductsRepository, IDisposable
	{
		private NorthwindContext _context;

		public ProductsRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Products.CountAsync();
		}

		public async Task<IEnumerable<Products>> GetProducts(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Products
							.Include(s => s.Suppliers)
							.Include(c => c.Categories)
							.OrderBy(c => c.ProductId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Products
										.CountAsync();

				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Products
									 .Include(s => s.Suppliers)
									 .Include(c => c.Categories)
									 .OrderBy(c => c.ProductId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Products> GetProduct(int productId)
		{
			return await _context.Products
								 .Include(s => s.Suppliers)
								 .Include(c => c.Categories)
								 .Where(c => c.ProductId == productId).FirstOrDefaultAsync();
		}

		public async void AddProduct(Products productToAdd)
		{
			if (productToAdd == null)
			{
				throw new ArgumentNullException(nameof(productToAdd));
			}

			await _context.AddAsync(productToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> ProductExits(int productId)
		{
			return (await _context.Products.AnyAsync(e => e.ProductId == productId));
		}

		public void DeleteProduct(Products productToDelete)
		{
			if (productToDelete == null)
			{
				throw new ArgumentNullException(nameof(productToDelete));
			}

			_context.Remove(productToDelete);
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
