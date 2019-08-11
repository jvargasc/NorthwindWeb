using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Models;

namespace Northwind.API.Services
{
	public class RegionRepository : IRegionRepository
	{
		private NorthwindContext _context;

		public RegionRepository(NorthwindContext context)
		{
			_context = context;
		}

		public IEnumerable<Region> GetRegions()
		{
			return _context.Region 
					.OrderBy(c => c.RegionId).ToList();
		}
	}
}
