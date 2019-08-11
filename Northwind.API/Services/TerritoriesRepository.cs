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

		public IEnumerable<Territories> GetTerritories()
		{
			return _context.Territories
					.OrderBy(c => c.TerritoryId).ToList();
		}
	}
}
