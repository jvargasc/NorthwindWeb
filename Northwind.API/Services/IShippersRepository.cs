using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface IShippersRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Shippers>> GetShippers(int page = 0, int itemsPerPage = 0);
		Task<Shippers> GetShipper(int shipperId);
		void AddShipper(Shippers shipperToAdd);
		Task<bool> SaveChanges();
		Task<bool> ShipperExits(int shipperId);
		void DeleteShipper(Shippers shipperToDelete);
	}
}