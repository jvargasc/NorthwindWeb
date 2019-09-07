using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IOrdersRepository
	{
		Task<IEnumerable<Orders>> GetOrders();
		Task<Orders> GetOrder(int orderId);
	}
}
