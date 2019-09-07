using Northwind.API.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Services
{
	public interface ITerritoriesRepository
	{
		Task<IEnumerable<Territories>> GetTerritories();
		Task<Territories> GetTerritory(string territoryId);
	}
}
