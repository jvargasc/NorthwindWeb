using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface IProductsRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Products>> GetProducts(int page = 0, int itemsPerPage = 0);
		Task<Products> GetProduct(int productId);
		void AddProduct(Products productToAdd);
		Task<bool> SaveChanges();
		Task<bool> ProductExits(int productId);
		void DeleteProduct(Products productToDelete);
	}
}