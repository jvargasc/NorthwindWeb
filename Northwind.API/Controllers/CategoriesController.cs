using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;
using System.Threading.Tasks;

namespace Northwind.API.Controllers
{
	[Route("api/categories")]
	public class CategoriesController : Controller
    {
		private ICategoriesRepository _categoriesRepository;
		private readonly IMapper _mapper;

		public CategoriesController(ICategoriesRepository categoriesRepository,
									IMapper mapper)
		{
			_categoriesRepository = categoriesRepository;
			_mapper = mapper;
		}

		[HttpGet("getcategories")]
		public async Task<ActionResult<IEnumerable<CategoriesDto>>> GetCategories()
		{
			var categoriesEntities = await _categoriesRepository.GetCategories();
			var _results = _mapper.Map<IEnumerable<CategoriesDto>>(categoriesEntities);

			return Ok(_results);			
		}

		[HttpGet("getcategory/{categoryId}")]
		public async Task<ActionResult<Categories>> GetCategory(int categoryId)
		{
			var categoryEntity = await _categoriesRepository.GetCategory(categoryId);
			//var _result = _mapper.Map<Categories>(categoryEntity);

			return Ok(categoryEntity);
		}
	}
}