using Northwind.API.Entities;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IOrdersRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Orders>> GetOrders(int page = 0, int itemsPerPage = 0);
		Task<Orders> GetOrder(int orderId);
		Task<List<OrderDetails>> GetOrderDetails(int orderId);
		void AddOrder(Orders orderToAdd);
		Task<bool> SaveChanges();
		Task<bool> OrderExits(int orderId);
		void DeleteOrder(Orders orderToDelete);
	}
}
