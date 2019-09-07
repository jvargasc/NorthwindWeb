using Microsoft.EntityFrameworkCore;
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

		public async Task<IEnumerable<Region>> GetRegions()
		{
			return await _context.Region 
					.OrderBy(c => c.RegionId).ToListAsync();
		}

		public async Task<Region> GetRegion(int regionId)
		{
			return await _context.Region
					.Where (c => c.RegionId == regionId).FirstOrDefaultAsync();
		}
	}
}
