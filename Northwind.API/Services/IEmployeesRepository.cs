using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface IEmployeesRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Employees>> GetEmployees(int page = 0, int itemsPerPage = 0);
		Task<Employees> GetEmployee(int employee);
		void AddEmployee(Employees employeeToAdd);
		Task<bool> SaveChanges();
		Task<bool> EmployeeExits(int employeeId);
		void DeleteEmployee(Employees employeeToDelete);
	}
}
