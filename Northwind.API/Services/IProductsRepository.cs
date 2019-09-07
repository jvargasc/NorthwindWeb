using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IProductsRepository
	{
		Task<IEnumerable<Products>> GetProducts();
		Task<Products> GetProduct(int productId);
	}
}