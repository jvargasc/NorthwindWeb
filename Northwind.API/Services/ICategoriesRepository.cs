using Northwind.API.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ICategoriesRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Categories>> GetCategories(int page = 0, int itemsPerPage = 0);
		Task<Categories> GetCategory(int categoryId);
		void AddCategory(Categories categoryToAdd);
		Task<bool> SaveChanges();
		Task<bool> CategoryExits(int categoryId);
		void DeleteCategory(Categories categoryToDelete);
	}
}
