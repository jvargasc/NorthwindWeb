using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class OrdersController : Controller
    {
        private readonly ServiceOrders _serviceOrders;
		private readonly IConfiguration _configuration;
		private IEnumerable<SelectListItem> shipViaListItems;
		private IEnumerable<SelectListItem> regionsListItems;

		public OrdersController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceOrders = new ServiceOrders(_configuration);
		}

		// GET: Orders
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{
			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceOrders.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Orders";

			var _results = await _serviceOrders.GetOrders(page, itemsPerPage);

			return View(_results);
		}

		// GET: Orders/Details/5
		public async Task<IActionResult> Details(int? orderId)
        {
            if (orderId == null)
                return NotFound();

			var _result = await _serviceOrders.GetOrder(orderId.Value);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Orders/Create
        public async Task<IActionResult> Create()
        {
			//List<Shippers> shippersList = await Utilities.GetShippers(_configuration);
			//ViewBag.Url = Utilities.GetUrl(_configuration);
			//ViewBag.ShipVia = JsonConvert.SerializeObject(shippersList);

			ViewData["Url"] = Utilities.GetUrl(_configuration);

			shipViaListItems = await Utilities.FillShipViaCollection(_configuration);

			ViewData["ShipVia"] = shipViaListItems;

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View();
        }

        // POST: Orders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,OrderId,CustomerId,EmployeeId,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
		public async Task<IActionResult> Create([FromForm] OrdersForCreation ordersForCreation,
												IFormCollection formCollection)
		{
			if (ModelState.IsValid)
			{
				ordersForCreation.OrderDetails = Utilities.GetOrderDetails(0, formCollection);

				await _serviceOrders.CreateOrder(ordersForCreation);
				return RedirectToAction(nameof(Index));
			}

			//ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", orders.EmployeeId);

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(ordersForCreation);
		}

		// GET: Orders/Edit/5
		public async Task<IActionResult> Edit(int? orderId)
        {
            if (orderId == null)
                return NotFound();

            var order = await _serviceOrders.GetOrder(orderId.Value);
            if (order == null)
                return NotFound();

			//List<Shippers> shippersList = await Utilities.GetShippers(_configuration);
			//ViewBag.Url = Utilities.GetUrl(_configuration);
			//ViewBag.ShipVia = JsonConvert.SerializeObject(shippersList);

			ViewData["Url"] = Utilities.GetUrl(_configuration);

			shipViaListItems = await Utilities.FillShipViaCollection(_configuration);

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			ViewData["ShipVia"] = shipViaListItems;

			return View(order);
        }

        // POST: Orders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,OrderId,CustomerId,EmployeeId,OrderDate,RequiredDate,ShippedDate,ShipVia,Freight,ShipName,ShipAddress,ShipCity,ShipRegion,ShipPostalCode,ShipCountry")] Orders orders)
		public async Task<IActionResult> Edit(int orderId, [FromForm] OrdersForUpdate order,
											  IFormCollection formCollection)
		{
            if (orderId != order.OrderId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
					order.OrderDetails = Utilities.GetOrderDetails(orderId, formCollection);

					await _serviceOrders.UpdateOrder(order);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if ( await OrderExists(order.OrderId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { orderId = orderId });
			}
            
			//ViewData["EmployeeId"] = new SelectList(_context.Employees, "Id", "Id", orders.EmployeeId);

            return View(order);
        }

        // GET: Orders/Delete/5
        public async Task<IActionResult> Delete(int? orderId)
        {
            if (orderId == null)
                return NotFound();

            var order = await _serviceOrders.GetOrder(orderId.Value);
            if (order == null)
                return NotFound();

            return View(order);
        }

        // POST: Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int orderId)
        {
            await _serviceOrders.DeleteOrder(orderId);
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> OrderExists(int orderId)
        {
            return await _serviceOrders.OrderExists(orderId);
        }

	}
}
