using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Models;
using Microsoft.EntityFrameworkCore;

namespace Northwind.API.Services
{
	public class EmployeesRepository : IEmployeesRepository
	{
		private readonly NorthwindContext _context;

		public EmployeesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Employees>> GetEmployees()
		{
			return await _context.Employees
				.OrderBy(c => c.EmployeeId).ToListAsync();
		}

		public async Task<Employees> GetEmployee(int employeeId)
		{
			return await _context.Employees
				.Where(c => c.EmployeeId == employeeId).FirstOrDefaultAsync();
		}

	}
}
