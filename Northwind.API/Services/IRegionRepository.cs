using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface IRegionRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Regions>> GetRegions(int page = 0, int itemsPerPage = 0);
		Task<Regions> GetRegion(int regionId);
		void AddRegion(Regions regionToAdd);
		Task<bool> SaveChanges();
		Task<bool> RegionExits(int regionId);
		void DeleteRegion(Regions regionToDelete);
		Task<int> GetNewId();
	}
}