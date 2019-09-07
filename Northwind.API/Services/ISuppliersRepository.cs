using Northwind.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ISuppliersRepository
	{
		Task<IEnumerable<Suppliers>> GetSuppliers();
		Task<Suppliers> GetSupplier(int supplier);
	}
}
