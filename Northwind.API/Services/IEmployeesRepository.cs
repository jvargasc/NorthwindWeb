using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IEmployeesRepository
	{
		Task<IEnumerable<Employees>> GetEmployees();
		Task<Employees> GetEmployee(int Employee);
	}
}
