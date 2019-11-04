using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class SuppliersController : Controller
    {
        private readonly ServiceSuppliers _serviceSuppliers;
		private readonly IConfiguration _configuration;
		private IEnumerable<SelectListItem> regionsListItems;

		public SuppliersController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceSuppliers = new ServiceSuppliers(_configuration);
		}

		// GET: Suppliers
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{

			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceSuppliers.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Suppliers";

			var _results = await _serviceSuppliers.GetSuppliers(page, itemsPerPage);

			return View(_results);
		}

		// GET: Suppliers/Details/5
		public async Task<IActionResult> Details(int? supplierId)
        {
            if (supplierId == null)
                return NotFound();

			var _result = await _serviceSuppliers.GetSupplier(supplierId.Value);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Suppliers/Create
        public async Task<IActionResult> Create()
        {
			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;
			return View();
        }

        // POST: Suppliers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,SupplierId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage")] Suppliers suppliers)
		public async Task<IActionResult> Create([FromForm] SuppliersForCreation supplier)
		{
            if (ModelState.IsValid)
            {
				await _serviceSuppliers.CreateSupplier(supplier);
                return RedirectToAction(nameof(Index));
            }
            return View(supplier);
        }

        // GET: Suppliers/Edit/5
        public async Task<IActionResult> Edit(int? supplierId)
        {
            if (supplierId == null)
                return NotFound();

            var supplier = await _serviceSuppliers.GetSupplier(supplierId.Value);
            if (supplier == null)
                return NotFound();

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(supplier);
        }

        // POST: Suppliers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,SupplierId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax,HomePage")] Suppliers suppliers)
		public async Task<IActionResult> Edit(int supplierId, [FromForm] SuppliersForUpdate supplier)
		{
            if (supplierId != supplier.SupplierId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
					await _serviceSuppliers.UpdateSupplier(supplier);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ( await SuppliersExists(supplier.SupplierId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { supplierId = supplierId });
			}
            return View(supplier);
        }

        // GET: Suppliers/Delete/5
        public async Task<IActionResult> Delete(int? supplierId)
        {
            if (supplierId == null)
                return NotFound();

            var supplier = await _serviceSuppliers.GetSupplier(supplierId.Value);
            if (supplier == null)
                return NotFound();

            return View(supplier);
        }

        // POST: Suppliers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int supplierId)
        {
            await _serviceSuppliers.DeleteSupplier(supplierId);
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SuppliersExists(int supplierId)
        {
            return await _serviceSuppliers.SupplierExists(supplierId);
        }
    }
}
