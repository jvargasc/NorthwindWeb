using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;
using System;

namespace Northwind.API.Services
{
	public class CategoriesRepository : ICategoriesRepository, IDisposable
	{
		private NorthwindContext _context;

		public CategoriesRepository(NorthwindContext context)
		{
			_context = context;
		}
		public async Task<int> GetCount()
		{
			return await _context.Categories.CountAsync();
		}

		public async Task<IEnumerable<Categories>> GetCategories(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Categories
						.OrderBy(c => c.CategoryId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Categories.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Categories
						.OrderBy(c => c.CategoryId).Skip(pageToSkip).Take(pageToTake).ToListAsync();

			}
		}

		public async Task<Categories> GetCategory(int categoryId)
		{
			return await _context.Categories
					.Where(c => c.CategoryId == categoryId)
					.FirstOrDefaultAsync();
		}

		public void AddCategory(Categories categoryToAdd)
		{
			if (categoryToAdd == null)
			{
				throw new ArgumentNullException(nameof(categoryToAdd));
			}

			_context.Add(categoryToAdd);
		}

		public async Task<bool> CategoryExits(int categoryId)
		{
			return (await _context.Categories.AnyAsync(e => e.CategoryId == categoryId));
		}
		
		public void DeleteCategory(Categories categoryToDelete)
		{
			if (categoryToDelete == null)
			{
				throw new ArgumentNullException(nameof(categoryToDelete));
			}

			_context.Remove(categoryToDelete);
		}

		public async Task<bool> SaveChanges()
		{
			// return true if 1 or more entities were changed
			return (await _context.SaveChangesAsync() > 0);
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
