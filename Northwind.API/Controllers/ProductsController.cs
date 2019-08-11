using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/products")]
	public class ProductsController : Controller
    {
		private readonly IProductsRepository _productsRepository;

		public ProductsController(IProductsRepository productsRepository )
		{
			_productsRepository = productsRepository;
		}

		[HttpGet("getproducts")]
		public IActionResult GetProducts()
        {
			var productsEntities = _productsRepository.GetProducts();
			var _results = new List<ProductsDto>();

			foreach(var item in productsEntities)
			{
				_results.Add(new ProductsDto
				{

					ProductId = item.ProductId,
					ProductName = item.ProductName,
					SupplierId = item.SupplierId,
					CategoryId = item.CategoryId,
					QuantityPerUnit = item.QuantityPerUnit,
					UnitPrice = item.UnitPrice,
					UnitsInStock = item.UnitsInStock,
					UnitsOnOrder = item.UnitsOnOrder,
					ReorderLevel = item.ReorderLevel,
					Discontinued = item.Discontinued,
					Category = item.Category,
					Supplier = item.Supplier
				});

			}

			return Ok(_results);
        }
    }
}