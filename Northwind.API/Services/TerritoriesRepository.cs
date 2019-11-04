using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;

namespace Northwind.API.Services
{
	public class TerritoriesRepository : ITerritoriesRepository, IDisposable
	{
		private NorthwindContext _context;

		public TerritoriesRepository(NorthwindContext context)
		{
			_context = context;
		}

		public async Task<int> GetCount()
		{
			return await _context.Territories.CountAsync();
		}

		public async void AddTerritory(Territories territoryToAdd)
		{
			if (territoryToAdd == null)
			{
				throw new ArgumentNullException(nameof(territoryToAdd));
			}

			await _context.AddAsync(territoryToAdd);
		}

		public void DeleteTerritory(Territories territoryToDelete)
		{
			if(territoryToDelete == null)
			{
				throw new ArgumentNullException(nameof(territoryToDelete));
			}

			_context.Remove(territoryToDelete);
		}

		public async Task<IEnumerable<Territories>> GetTerritories(int page = 0, int itemsPerPage = 0)
		{
			if (page == 0 || itemsPerPage == 0)
			{
				return await _context.Territories									 
									 .Include(r => r.Regions)
									 .OrderBy(c => c.TerritoryId).Take(10).ToListAsync();
			}
			else
			{
				var recordCount = await _context.Territories.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;
				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Territories
									 .Include(r => r.Regions)
									 .OrderBy(c => c.TerritoryId).Skip(pageToSkip).Take(pageToTake).ToListAsync();
			}
		}

		public async Task<Territories> GetTerritory(string territoryId)
		{
			return await _context.Territories
								 .Include(r => r.Regions)
								 .Where(c => c.TerritoryId == territoryId)
								 .FirstOrDefaultAsync();
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> TerritoryExits(string territoryId)
		{
			return (await _context.Territories.AnyAsync(e => e.TerritoryId == territoryId));
		}

		public async Task<string> GetNewId()
		{
			string maxIdStr = await _context.Territories.Select(t => t.TerritoryId).MaxAsync();
			int maxId = Int32.Parse(maxIdStr); ;
			maxId++;

			maxIdStr = maxId.ToString();

			if (maxIdStr.Length == 4)
				maxIdStr = "0" + maxIdStr;

			return (maxIdStr);
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
