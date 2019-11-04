using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface ICustomersRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Customers>> GetCustomers(int page = 0, int itemsPerPage = 0);
		Task<Customers> GetCustomer(string customerId);
		void AddCustomer(Customers customerToAdd);
		Task<bool> SaveChanges();
		Task<bool> CustomerExits(string customerId);
		void DeleteCustomer(Customers customerToDelete);
	}
}