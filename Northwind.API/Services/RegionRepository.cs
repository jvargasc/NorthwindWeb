using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public class RegionRepository : IRegionRepository, IDisposable
	{
		private NorthwindContext _context;

		public RegionRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Regions.CountAsync();
		}

		public async Task<IEnumerable<Regions>> GetRegions(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Regions
					.OrderBy(c => c.RegionId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Regions.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Regions.OrderBy(c => c.RegionId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Regions> GetRegion(int regionId)
		{
			return await _context.Regions
						.Where(c => c.RegionId == regionId).FirstOrDefaultAsync();
		}

		public async void AddRegion(Regions regionToAdd)
		{
			if (regionToAdd == null)
			{
				throw new ArgumentNullException(nameof(regionToAdd));
			}

			await _context.AddAsync(regionToAdd);
		}

		public async Task<bool> SaveChanges()
		{
				return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> RegionExits(int regionId)
		{
			return (await _context.Regions.AnyAsync(e => e.RegionId == regionId));
		}

		public void DeleteRegion(Regions regionToDelete)
		{
			if (regionToDelete == null)
			{
				throw new ArgumentNullException(nameof(regionToDelete));
			}

			_context.Remove(regionToDelete);
		}

		public async Task<int> GetNewId()
		{
			int maxId = await _context.Regions.Select(r => r.RegionId).MaxAsync();
			maxId++;

			return (maxId);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}

	}
}
