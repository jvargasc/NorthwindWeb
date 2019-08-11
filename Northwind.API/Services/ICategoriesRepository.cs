using Northwind.API.Models;
using System.Collections.Generic;

namespace Northwind.API.Services
{
	public interface ICategoriesRepository
	{
		IEnumerable<Categories> GetCategories();
	}
}
