using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public interface ITerritoriesRepository
	{
		Task<int> GetCount();
		Task<IEnumerable<Territories>> GetTerritories(int page = 0, int itemsPerPage = 0);
		Task<Territories> GetTerritory(string territoryId);
		void AddTerritory(Territories territoryToAdd);
		Task<bool> SaveChanges();
		Task<bool> TerritoryExits(string territoryId);
		void DeleteTerritory(Territories territoryToDelete);
		Task<string> GetNewId();
	}
}