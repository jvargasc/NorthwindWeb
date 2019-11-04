using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;

namespace Northwind
{
	public class CategoriesController : Controller
	{
		private readonly ServiceCategories _serviceCategories;
		private readonly IConfiguration _configuration;
		//private static byte[] TmpPicture;
		private static List<Utilities.PictureFile> TmpPicture = new List<Utilities.PictureFile>();

		public CategoriesController(IConfiguration configuration)
		{
			_configuration = configuration;
			_serviceCategories = new ServiceCategories(_configuration);
		}

		// GET: Categories
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{

			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceCategories.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Categories";

			var _results = await _serviceCategories.GetCategories(page, itemsPerPage);
			
			return View(_results);
		}

		// GET: Categories/Details/5
		public async Task<IActionResult> Details(int? categoryId)
        {
            if (categoryId == null)
				return NotFound();

			var category = new ServiceCategories(_configuration);
			var _result = await category.GetCategory(categoryId.Value);

			if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Categories/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,CategoryId,CategoryName,Description,Picture64")] Categories categories)
		public async Task<IActionResult> Create([FromForm] CategoriesForCreation category, IFormFile imgFile)
		{
            if (ModelState.IsValid)
            {
				var newImgFile = await Utilities.ConvertPictureToBytes(imgFile);
				category.Picture = newImgFile;
				//var newCategory =

				await _serviceCategories.CreateCategory(category);

                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? categoryId)
        {
            if (categoryId == null)
                return NotFound();

			var category = await _serviceCategories.GetCategory(categoryId.Value);
			if (category == null)
                return NotFound();

			TmpPicture.Add(new Utilities.PictureFile() { Id = category.CategoryId.ToString(),
								 TmpPicture = category.Picture });

			return View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryId,CategoryName,Description,Picture64")] Categories categories)
		public async Task<IActionResult> Edit(int categoryId, [FromForm] CategoriesForUpdate category, IFormFile imgFile)
		{
            if (categoryId != category.CategoryId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
					if (imgFile == null)
					{
						category.Picture = TmpPicture
											.Find(r => r.Id == category.CategoryId.ToString() )
											.TmpPicture;
					}
					else
					{
						var newImgFile = await Utilities.ConvertPictureToBytes(imgFile);
						category.Picture = newImgFile;
					}

					await _serviceCategories.UpdateCategory(category);
					//TmpPicture = new byte[0];
					var TmpPic = TmpPicture
								 .Find(r => r.Id == category.CategoryId.ToString());

					TmpPicture.Remove(TmpPic);
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (await CategoryExists(category.CategoryId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { categoryId = categoryId });
			}
            return View(category);
        }

        // GET: Categories/Delete/5
        public async Task<IActionResult> Delete(int? categoryId)
        {
            if (categoryId == null)
                return NotFound();

			var categories = await _serviceCategories.GetCategory(categoryId.Value);

			if (categories == null)
                return NotFound();

            return View(categories);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int categoryId)
        {
			await _serviceCategories.DeleteCategory(categoryId);

			return RedirectToAction(nameof(Index));
        }

        private async Task<bool> CategoryExists(int categoryId)
        {
			return await _serviceCategories.CategoryExists(categoryId);
		}
    }
}
