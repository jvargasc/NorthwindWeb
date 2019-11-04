using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class ShippersController : Controller
    {
        private readonly ServiceShippers _serviceShippers;
		private readonly IConfiguration _configuration;

		public ShippersController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceShippers = new ServiceShippers(_configuration);
		}

		// GET: Shippers
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{
			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceShippers.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Shippers";

			var _results = await _serviceShippers.GetShippers(page, itemsPerPage);

			return View(_results);
		}

		// GET: Shippers/Details/5
		public async Task<IActionResult> Details(int? shipperId)
        {
            if (shipperId == null)
                return NotFound();

			var _result = await _serviceShippers.GetShipper(shipperId.Value);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Shippers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shippers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,ShipperId,CompanyName,Phone")] Shippers shippers)
		public async Task<IActionResult> Create([FromForm] ShippersForCreation shipper)
		{
            if (ModelState.IsValid)
            {
				await _serviceShippers.CreateShipper(shipper);
                return RedirectToAction(nameof(Index));
            }
            return View(shipper);
        }

        // GET: Shippers/Edit/5
        public async Task<IActionResult> Edit(int? shipperId)
        {
            if (shipperId == null)
                return NotFound();

            var shippers = await _serviceShippers.GetShipper(shipperId.Value);
            if (shippers == null)
                return NotFound();

            return View(shippers);
        }

        // POST: Shippers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,ShipperId,CompanyName,Phone")] Shippers shippers)
		public async Task<IActionResult> Edit(int shipperId, [FromForm] ShippersForUpdate shipper)
		{
            if (shipperId != shipper.ShipperId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceShippers.UpdateShipper(shipper);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await ShippersExists(shipper.ShipperId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { shipperId = shipperId });
			}
            return View(shipper);
        }

        // GET: Shippers/Delete/5
        public async Task<IActionResult> Delete(int? shipperId)
        {
            if (shipperId == null)
                return NotFound();

            var shipper = await _serviceShippers.GetShipper(shipperId.Value);
            if (shipper == null)
                return NotFound();

            return View(shipper);
        }

        // POST: Shippers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int shipperId)
        {
            await _serviceShippers.DeleteShipper(shipperId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ShippersExists(int shipperId)
        {
            return await _serviceShippers.ShipperExists(shipperId);
        }
    }
}
