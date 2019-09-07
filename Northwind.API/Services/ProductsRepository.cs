using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public class ProductsRepository : IProductsRepository
	{
		private NorthwindContext _context;

		public ProductsRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Products>> GetProducts()
		{
			return await _context.Products
				.OrderBy(c => c.ProductId).ToListAsync();
		}

		public async Task<Products> GetProduct(int productId)
		{
			return await _context.Products
				.Where(c => c.ProductId == productId).FirstOrDefaultAsync();
		}
	}
}
