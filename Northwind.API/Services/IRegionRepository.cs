using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface IRegionRepository
	{
		Task<IEnumerable<Region>> GetRegions();
		Task<Region> GetRegion(int regionId);
	}
}
