using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/products")]
	public class ProductsController : Controller
    {
		private readonly IProductsRepository _productsRepository;
		private readonly IMapper _mapper;

		public ProductsController(IProductsRepository productsRepository,
									IMapper mapper)
		{
			_productsRepository = productsRepository;
			_mapper = mapper;
		}

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _productsRepository.GetCount());
		}

		[HttpGet("getproducts")]
		public async Task<ActionResult<IEnumerable<Models.ProductsDto>>> GetProducts(int page = 0, int itemsPerPage = 0)
		{
			var productsEntities = await _productsRepository.GetProducts(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.ProductsDto>>(productsEntities);

			return Ok(_results);
        }
		
		[HttpGet("getproduct/{productId}")]
		public async Task<ActionResult<Models.Products>> GetProduct(int productId)
		{
			var productEntity = await _productsRepository.GetProduct(productId);
			var _result = _mapper.Map<Products>(productEntity);

			return Ok(_result);
		}

		[HttpGet("productexists/{productId}")]
		public async Task<ActionResult<bool>> ProductExists(int productId)
		{
			var productExists = await _productsRepository.ProductExits(productId);

			return Ok(productExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateProduct(
			[FromBody] Models.ProductsForCreation productToCreate)
		{
			if (productToCreate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var productEntity = _mapper.Map<Products>(productToCreate);
			_productsRepository.AddProduct(productEntity);

			await _productsRepository.SaveChanges();

			await _productsRepository.GetProduct(productEntity.ProductId);

			return Ok(CreatedAtRoute("GetProduct",
						new { productId = productEntity.ProductId },
						_mapper.Map<Models.Products>(productEntity)));

		}

		[HttpPut("{productId}")]
		public async Task<IActionResult> UpdateProduct(int productId,
			[FromBody] Models.ProductsForUpdate productToUpdate)
		{
			if (productToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var productEntity = await _productsRepository.GetProduct(productId);
			if (productEntity == null)
				return NotFound();

			productToUpdate.Categories = new Models.Categories()
			{
				CategoryId = productEntity.Categories.CategoryId,
				CategoryName = productEntity.Categories.CategoryName,
				Description = productEntity.Categories.Description,
				Picture = productEntity.Categories.Picture
			};

			productToUpdate.Suppliers = new Models.Suppliers()
			{
				Address = productEntity.Suppliers.Address,
				City = productEntity.Suppliers.City,
				CompanyName = productEntity.Suppliers.CompanyName,
				ContactName = productEntity.Suppliers.ContactName,
				ContactTitle = productEntity.Suppliers.ContactTitle,
				Country = productEntity.Suppliers.Country,
				Fax = productEntity.Suppliers.Fax,
				HomePage = productEntity.Suppliers.HomePage,
				Phone = productEntity.Suppliers.Phone,
				PostalCode = productEntity.Suppliers.PostalCode,
				RegionId = productEntity.Suppliers.RegionId,
				Regions = new Models.Regions()
				{
					RegionDescription = productEntity.Suppliers.Regions.RegionDescription,
					RegionId = productEntity.Suppliers.Regions.RegionId
				},
				SupplierId = productEntity.Suppliers.SupplierId
			};
				
			_mapper.Map(productToUpdate, productEntity);

			await _productsRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Products>(productEntity));
		}

		[HttpDelete("{productId}")]
		public async Task<IActionResult> DeleteProduct(int productId)
		{
			var productEntity = await _productsRepository.GetProduct(productId);
			if (productEntity == null)
				return NotFound();

			_productsRepository.DeleteProduct(productEntity);
			await _productsRepository.SaveChanges();

			return NoContent();
		}
	}
}