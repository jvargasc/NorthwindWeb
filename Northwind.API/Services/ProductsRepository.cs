using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class ProductsRepository : IProductsRepository
	{
		private NorthwindContext _context;

		public ProductsRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Products> GetProducts()
		{
			return _context.Products
				.OrderBy(c => c.ProductId).ToList();
		}
	}
}
