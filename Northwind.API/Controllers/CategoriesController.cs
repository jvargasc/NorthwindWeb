using AutoMapper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Drawing;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
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

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _categoriesRepository.GetCount());
		}

		[HttpGet("getcategories")]
		public async Task<ActionResult<IEnumerable<Models.CategoriesDto>>> GetCategories(int page = 0, int itemsPerPage = 0)
		{
			var categoriesEntities = await _categoriesRepository.GetCategories(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.CategoriesDto>>(categoriesEntities);

			return Ok(_results);			
		}

		[HttpGet("getcategory/{categoryId}")]
		public async Task<ActionResult<Models.Categories>> GetCategory(int categoryId)
		{
			var categoryEntity = await _categoriesRepository.GetCategory(categoryId);
			if (categoryEntity == null)
			{
				return NotFound();
			}

			return Ok(_mapper.Map<Categories>(categoryEntity));
		}

		[HttpGet("categoryexists/{categoryId}")]
		public async Task<ActionResult<bool>> CategoryExists(int categoryId)
		{
			bool categoryExists = await _categoriesRepository.CategoryExits(categoryId);

			return Ok(categoryExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCategory(
			[FromBody] Models.CategoriesForCreation categoriesForCreation)
		{
			// model validation 
			if (categoriesForCreation == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				// return 422 - Unprocessable Entity when validation fails
				return new UnprocessableEntityObjectResult(ModelState);
			}

			var categoryEntity = _mapper.Map<Categories>(categoriesForCreation);
			_categoriesRepository.AddCategory(categoryEntity);

			// save the changes
			await _categoriesRepository.SaveChanges();

			// Fetch the movie from the data store so the director is included
			await _categoriesRepository.GetCategory(categoryEntity.CategoryId);

			return Ok( CreatedAtRoute("GetCategory",
					new { categoryId = categoryEntity.CategoryId },
					_mapper.Map<Models.Categories>(categoryEntity)));
		}

		[HttpPut("{categoryId}")]
		public async Task<IActionResult> UpdateCategory(int categoryId,
			[FromBody] Models.CategoriesForUpdate categoryToUpdate)
		{
			// model validation 
			if (categoryToUpdate == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				// return 422 - Unprocessable Entity when validation fails
				return new UnprocessableEntityObjectResult(ModelState);
			}

			var categoryEntity = await _categoriesRepository.GetCategory(categoryId);
			if (categoryEntity == null)
			{
				return NotFound();
			}

			// map the inputted object into the movie entity
			// this ensures properties will get updated
			_mapper.Map(categoryToUpdate, categoryEntity);

			// call into UpdateMovie even though in our implementation 
			// this doesn't contain code - doing this ensures the code stays
			// reliable when other repository implemenations (eg: a mock 
			// repository) are used.
			//_categoriesRepository.Update(categoryEntity);

			await _categoriesRepository.SaveChanges();

			// return the updated movie, after mapping it
			return Ok(_mapper.Map<Models.Categories>(categoryEntity));
		}

		[HttpDelete("{categoryId}")]
		public async Task<IActionResult> DeleteCategory(int categoryId)
		{
			var categoryEntity = await _categoriesRepository.GetCategory(categoryId);
			if (categoryEntity == null)
			{
				return NotFound();
			}

			_categoriesRepository.DeleteCategory(categoryEntity);
			await _categoriesRepository.SaveChanges();

			return NoContent();
		}

	}
}