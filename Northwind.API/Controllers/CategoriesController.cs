using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
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
					Picture = item.Picture // "data:image/bmp;base64," + Convert.ToBase64String(TransformPicture(item.Picture)) 
				});
			}

			return Ok(_results);
        }

		private byte[] TransformPicture(byte[] Picture)
		{

			Byte[] image = new Byte[0];
			image = (byte[])Picture;

			MemoryStream ms = new MemoryStream();
			ms.Write(image, 78, image.Length - 78);

			// convert stream to string
			ms.Seek(0, SeekOrigin.Begin);
			StreamReader reader = new StreamReader(ms);
			string text = reader.ReadToEnd();

			return System.Text.Encoding.UTF8.GetBytes(text);
		}
	}
}