using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.API.Contexts;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public class EmployeesRepository : IEmployeesRepository, IDisposable
	{
		private NorthwindContext _context;

		public EmployeesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Employees.CountAsync();
		}

		public async Task<IEnumerable<Employees>> GetEmployees(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Employees
								.Include(r => r.Regions)
								.OrderBy(c => c.EmployeeId)
								.Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Employees.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Employees
								.Include(r => r.Regions)
								.OrderBy(c => c.EmployeeId)
								.Skip(pageToSkip)
								.Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Employees> GetEmployee(int employeeId)
		{
			return await _context.Employees
				.Include(r => r.Regions)
				.Where(c => c.EmployeeId == employeeId).FirstOrDefaultAsync();
		}

		public async void AddEmployee(Employees employeeToAdd)
		{
			if (employeeToAdd == null)
			{
				throw new ArgumentNullException(nameof(employeeToAdd));
			}

			await _context.AddAsync(employeeToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> EmployeeExits(int employeeId)
		{
			return (await _context.Employees.AnyAsync(e => e.EmployeeId == employeeId));
		}

		public void DeleteEmployee(Employees employeeToDelete)
		{
			if (employeeToDelete == null)
			{
				throw new ArgumentNullException(nameof(employeeToDelete));
			}

			_context.Remove(employeeToDelete);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}
	}
}
