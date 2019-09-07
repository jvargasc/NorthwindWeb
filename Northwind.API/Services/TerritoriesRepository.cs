using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class TerritoriesRepository : ITerritoriesRepository
	{
		private NorthwindContext _context;

		public TerritoriesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<IEnumerable<Territories>> GetTerritories()
		{
			return await _context.Territories
					.OrderBy(c => c.TerritoryId).ToListAsync();
		}

		public async Task<Territories> GetTerritory(string territoryId)
		{
			return await _context.Territories
					.Where(c => c.TerritoryId == territoryId).FirstOrDefaultAsync();
		}
	}
}
