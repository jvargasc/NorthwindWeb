using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class EmployeesRepository : IEmployeesRepository
	{
		private readonly NorthwindContext _context;

		public EmployeesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Employees> GetEmployees()
		{
			return _context.Employees
				.OrderBy(c => c.EmployeeId).ToList();
		}
	}
}
