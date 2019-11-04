using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface ISuppliersRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Suppliers>> GetSuppliers(int page = 0, int itemsPerPage = 0);
		Task<Suppliers> GetSupplier(int supplierId);
		void AddSupplier(Suppliers supplierToAdd);
		Task<bool> SaveChanges();
		Task<bool> SupplierExits(int supplierId);
		void DeleteSupplier(Suppliers supplierToDelete);
	}
}
