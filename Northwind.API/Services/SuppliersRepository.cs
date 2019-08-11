using System.Collections.Generic;
using System.Linq;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class SuppliersRepository : ISuppliersRepository
	{
		private NorthwindContext _context;

		public SuppliersRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Suppliers> GetSuppliers()
		{
			return _context.Suppliers
					.OrderBy(c => c.SupplierId).ToList();
		}
	}
}
