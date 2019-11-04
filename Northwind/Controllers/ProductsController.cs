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
	public class ProductsController : Controller
    {
        private readonly ServiceProducts _serviceProducts;
		private readonly IConfiguration _configuration;
		private IEnumerable<SelectListItem> suppliersListItems;
		private IEnumerable<SelectListItem> categoriesListItems;

		public ProductsController(IConfiguration configuration)
		{
            _configuration = configuration;
			_serviceProducts = new ServiceProducts(_configuration);
		}

		// GET: Products
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{

			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceProducts.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Products";

			var _results = await _serviceProducts.GetProducts(page, itemsPerPage);

			return View(_results);
		}

		// GET: Products/Details/5
		public async Task<IActionResult> Details(int? productId)
        {
            if (productId == null)
                return NotFound();

			var _result = await _serviceProducts.GetProduct(productId.Value);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Products/Create
        public async Task<IActionResult> Create()
        {
			//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id");
			//ViewData["SupplierId"] = new SelectList(_context.Set<Suppliers>(), "Id", "Id");

			ViewData["Url"] = Utilities.GetUrl(_configuration);

			suppliersListItems = await Utilities.FillSuppliersCollection(_configuration);
			categoriesListItems = await Utilities.FillCategoriesCollection(_configuration);

			ViewData["Suppliers"] = suppliersListItems;
			ViewData["Categories"] = categoriesListItems;

			return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(
			[FromForm] ProductsForCreation product)
        {
            if (ModelState.IsValid)
            {
                await _serviceProducts.CreateProduct(product);

                return RedirectToAction(nameof(Index));
            }
            //ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", products.CategoryId);
            //ViewData["SupplierId"] = new SelectList(_context.Set<Suppliers>(), "Id", "Id", products.SupplierId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? productId)
        {
            if (productId == null)
                return NotFound();

            var product = await _serviceProducts.GetProduct(productId.Value);
            if (product == null)
                return NotFound();

			//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", products.CategoryId);
			//ViewData["SupplierId"] = new SelectList(_context.Set<Suppliers>(), "Id", "Id", products.SupplierId);

			ViewData["Url"] = Utilities.GetUrl(_configuration);

			suppliersListItems = await Utilities.FillSuppliersCollection(_configuration);
			categoriesListItems = await Utilities.FillCategoriesCollection(_configuration);

			ViewData["Suppliers"] = suppliersListItems;
			ViewData["Categories"] = categoriesListItems;

			return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int productId, 
			[FromForm] ProductsForUpdate product)
        {
            if (productId != product.ProductId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    await _serviceProducts.UpdateProduct(product);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (await ProductsExists(product.ProductId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { productId = productId });
			}
			//ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Id", products.CategoryId);
			//ViewData["SupplierId"] = new SelectList(_context.Set<Suppliers>(), "Id", "Id", products.SupplierId);

			return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? productId)
        {
            if (productId == null)
                return NotFound();

            var product = await _serviceProducts.GetProduct(productId.Value);
            if (product == null)
                return NotFound();

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int productId)
        {
            await _serviceProducts.DeleteProduct(productId);
            
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProductsExists(int productId)
        {
            return await _serviceProducts.ProductExists(productId);
        }
    }
}
