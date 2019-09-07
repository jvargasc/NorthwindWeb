using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class CategoriesRepository : ICategoriesRepository
	{
		private readonly NorthwindContext _context;

		public CategoriesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Categories>> GetCategories()
		{
			return await _context.Categories
					.OrderBy(c => c.CategoryId).ToListAsync();
		}

		public async Task<Categories> GetCategory(int categoryId)
		{
			return await _context.Categories
					.Where(c => c.CategoryId == categoryId)
					.FirstOrDefaultAsync();
		}
	}
}
