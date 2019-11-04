using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class RegionsController : Controller
    {
        private readonly ServiceRegion _serviceRegion;
		private readonly IConfiguration _configuration;

		public RegionsController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceRegion = new ServiceRegion(_configuration);
		}

		// GET: Regions
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{
			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceRegion.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Regions";

			var _results = await _serviceRegion.GetRegions(page, itemsPerPage);

			return View(_results);
		}

		// GET: Regions/Details/5
		public async Task<IActionResult> Details(int? regionId)
        {
            if (regionId == null)
                return NotFound();

			var _result = await _serviceRegion.GetRegion(regionId.Value);

            if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Regions/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Regions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,RegionId,RegionDescription")] Region region)
		public async Task<IActionResult> Create([FromForm] RegionForCreation region)
		{
            if (ModelState.IsValid)
            {
				await _serviceRegion.CreateRegion(region);
				
                return RedirectToAction(nameof(Index));
            }
            return View(region);
        }

        // GET: Regions/Edit/5
        public async Task<IActionResult> Edit(int? regionId)
        {
            if (regionId == null)
                return NotFound();

            var region = await _serviceRegion.GetRegion(regionId.Value);
            if (region == null)
                return NotFound();

			return View(region);
        }

        // POST: Regions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,RegionId,RegionDescription")] Region region)
		public async Task<IActionResult> Edit(int regionId, [FromForm] RegionForUpdate region)
		{
            if (regionId != region.RegionId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
				  	await _serviceRegion.UpdateRegion(region);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await RegionExists(region.RegionId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { regionId = regionId });
			}
            return View(region);
        }

        // GET: Regions/Delete/5
        public async Task<IActionResult> Delete(int? regionId)
        {
            if (regionId == null)
                return NotFound();

            var region = await _serviceRegion.GetRegion(regionId.Value);
            if (region == null)
                return NotFound();

            return View(region);
        }

        // POST: Regions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int regionId)
        {
            await _serviceRegion.DeleteRegion(regionId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> RegionExists(int regionId)
        {
            return await _serviceRegion.RegionExists(regionId);
        }
    }
}
