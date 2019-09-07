using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public class SuppliersRepository : ISuppliersRepository
	{
		private NorthwindContext _context;

		public SuppliersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Suppliers>> GetSuppliers()
		{
			return await _context.Suppliers
					.OrderBy(c => c.SupplierId).ToListAsync();
		}

		public async Task<Suppliers> GetSupplier(int supplierId)
		{
			return await _context.Suppliers
					.Where(c => c.SupplierId == supplierId).FirstOrDefaultAsync();
		}
	}
}
