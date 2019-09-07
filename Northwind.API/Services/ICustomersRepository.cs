using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ICustomersRepository
	{
		Task<IEnumerable<Customers>> GetCustomers();
		Task<Customers> GetCustomer(string customerId);
	}
}