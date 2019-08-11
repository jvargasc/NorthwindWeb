using Northwind.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ISuppliersRepository
	{
		IEnumerable<Suppliers> GetSuppliers();
	}
}
