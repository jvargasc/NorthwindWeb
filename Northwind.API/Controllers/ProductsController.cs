using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
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

		[HttpGet("getproducts")]
		public async Task<ActionResult> GetProducts()
        {
			var productsEntities = await _productsRepository.GetProducts();
			var _results = _mapper.Map<IEnumerable<ProductsDto>>(productsEntities);
			//var _results = new List<ProductsDto>();

			//foreach(var item in productsEntities)
			//{
			//	_results.Add(new ProductsDto
			//	{

			//		ProductId = item.ProductId,
			//		ProductName = item.ProductName,
			//		SupplierId = item.SupplierId,
			//		CategoryId = item.CategoryId,
			//		QuantityPerUnit = item.QuantityPerUnit,
			//		UnitPrice = item.UnitPrice,
			//		UnitsInStock = item.UnitsInStock,
			//		UnitsOnOrder = item.UnitsOnOrder,
			//		ReorderLevel = item.ReorderLevel,
			//		Discontinued = item.Discontinued,
			//		Category = item.Category,
			//		Supplier = item.Supplier
			//	});

			return Ok(_results);
        }
		
		[HttpGet("getproduct/{productId}")]
		public async Task<ActionResult> GetProduct(int productId)
		{
			var productEntity = await _productsRepository.GetProduct(productId);
			var _result = _mapper.Map<Products>(productEntity);

			return Ok(_result);
		}
	}
}