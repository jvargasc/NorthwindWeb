using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ICategoriesRepository
	{
		Task<IEnumerable<Categories>> GetCategories();
		Task<Categories> GetCategory(int categoryId);
	}
}
