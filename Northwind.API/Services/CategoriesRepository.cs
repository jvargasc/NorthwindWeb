using System.Collections.Generic;
using System.Linq;
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

		public IEnumerable<Categories> GetCategories()
		{
			return _context.Categories
					.OrderBy(c => c.CategoryId).ToList();
		}
	}
}
