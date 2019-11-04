using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class TerritoriesController : Controller
    {
        private readonly ServiceTerritories _serviceTerritories;
		private readonly IConfiguration _configuration;
		private IEnumerable<SelectListItem> regionsListItems;

		public TerritoriesController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceTerritories = new ServiceTerritories(_configuration);
		}

		// GET: Territories
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{

			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceTerritories.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Territories";

			var _results = await _serviceTerritories.GetTerritories(page, itemsPerPage);

			return View(_results);
		}

		// GET: Territories/Details/5
		public async Task<IActionResult> Details(string territoryId)
        {
			if (string.IsNullOrEmpty(territoryId))
                return NotFound();

			var _result = await _serviceTerritories.GetTerritory(territoryId);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Territories/Create
        public async Task<IActionResult> Create()
        {
			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View();
        }

        // POST: Territories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,TerritoryId,TerritoryDescription,RegionId")] Territories territories)
		public async Task<IActionResult> Create([FromForm] TerritoriesForCreation territory)
		{
            if (ModelState.IsValid)
            {
				await _serviceTerritories.CreateTerritory(territory);
                return RedirectToAction(nameof(Index));
            }

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(territory);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(string territoryId)
        {
            if (string.IsNullOrEmpty(territoryId))
                return NotFound();

            var territories = await _serviceTerritories.GetTerritory(territoryId);
            if (territories == null)
                return NotFound();

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(territories);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,TerritoryId,TerritoryDescription,RegionId")] Territories territories)
		public async Task<IActionResult> Edit(string territoryId, [FromForm] TerritoriesForUpdate territory)
		{
            if (territoryId != territory.TerritoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceTerritories.UpdateTerritory(territory);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ( await TerritoriesExists(territory.TerritoryId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { territoryId = territoryId });
			}
            //ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id", territories.RegionId);
            return View(territory);
        }

        // GET: Territories/Delete/5
        public async Task<IActionResult> Delete(string territoryId)
        {
            if (string.IsNullOrEmpty(territoryId))
                return NotFound();

            var territory = await _serviceTerritories.GetTerritory(territoryId);
            if (territory == null)
                return NotFound();

            return View(territory);
        }

        // POST: Territories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string territoryId)
        {
            await _serviceTerritories.DeleteTerritory(territoryId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> TerritoriesExists(string territoryId)
        {
            return await _serviceTerritories.TerritoryExists(territoryId);
        }
    }
}
