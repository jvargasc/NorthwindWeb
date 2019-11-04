using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;
using System.Linq;
using Newtonsoft.Json;
using System;

namespace Northwind.Controllers
{
	public class CustomersController : Controller
    {
        private readonly ServiceCustomers _serviceCustomers;
		private readonly IConfiguration _configuration;
		//public IEnumerable<SelectListItem> Regions { get; set; }
		private IEnumerable<SelectListItem> regionsListItems;

		public CustomersController(IConfiguration configuration)
        {
			_configuration = configuration;
			_serviceCustomers = new ServiceCustomers(_configuration);
		}

		// GET: Customers
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{
			DistributionPerPage distributionPerPage = new DistributionPerPage();
			
			distributionPerPage.recordCount = await _serviceCustomers.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Customers";

			var _results = await _serviceCustomers.GetCustomers(page, itemsPerPage);

			return View(_results);
		}

		// GET: Customers/Details/5
		//public async Task<IActionResult> Details(string? id)
		public async Task<IActionResult> Details(string customerId)
		{
			if (string.IsNullOrEmpty(customerId))
			{
                return NotFound();
            }

            var customer = await new ServiceCustomers(_configuration)
				.GetCustomer(customerId);
            if (customer == null)
            {
                return NotFound();
            }

			return View(customer);
        }

        // GET: Customers/Create
        public async Task<IActionResult> Create()
        {

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;
			return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] CustomersForCreation customer)
		public async Task<IActionResult> Create( [FromForm] CustomersForCreation customer)
        {
            if (ModelState.IsValid)
            {
				await _serviceCustomers.CreateCustomer(customer);

				return RedirectToAction(nameof(Index));
            }
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return NotFound();
            }

            var customer = await _serviceCustomers.GetCustomer(customerId);
            if (customer == null)
            {
                return NotFound();
            }
			
			//Region = customer.Region;
			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,CustomerId,CompanyName,ContactName,ContactTitle,Address,City,Region,PostalCode,Country,Phone,Fax")] Customers customers)
		public async Task<IActionResult> Edit(string customerId, 
			[FromForm] CustomersForUpdate customer)
		{
			if (customerId != customer.CustomerId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
					await _serviceCustomers.UpdateCustomer(customer);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await CustomersExists(customer.CustomerId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return View(customer);
				return RedirectToAction("Detail", new { customerId = customerId });
			}
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(string customerId)
        {
            if (string.IsNullOrEmpty(customerId))
            {
                return NotFound();
            }

            var customer = await _serviceCustomers.GetCustomer(customerId);
            if (customer == null)
                return NotFound();

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string customerId)
        {
			await _serviceCustomers.DeleteCustomer(customerId);
         
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CustomersExists(string customerId)
        {
            return await _serviceCustomers.CustomerExists(customerId);
        }

	}
}
