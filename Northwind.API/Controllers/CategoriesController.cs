using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/categories")]
	public class CategoriesController : Controller
    {
		private ICategoriesRepository _categoriesRepository;

		public CategoriesController(ICategoriesRepository categoriesRepository)
		{
			_categoriesRepository = categoriesRepository;
		}

		[HttpGet("getcategories")]
		public IActionResult GetCategories()
		{
			//
			var categoriesEntities = _categoriesRepository.GetCategories();
			var _results = new List<CategoriesDto>();

			foreach (var item in categoriesEntities)
			{
				_results.Add(new CategoriesDto
				{
					CategoryId  = item.CategoryId,
					CategoryName = item.CategoryName,
					Description = item.Description,
					Picture = item.Picture
				});
			}

			return Ok(_results);
        }
    }
}