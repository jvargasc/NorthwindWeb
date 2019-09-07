using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IShippersRepository
	{
	   Task<IEnumerable<Shippers>> GetShippers();
	   Task<Shippers> GetShipper(int shipperId);
	}
}