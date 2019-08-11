using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/shippers")]
	public class ShippersController : Controller
    {
		private readonly IShippersRepository _shippersRepository;

		public ShippersController(IShippersRepository shippersRepository )
		{
			_shippersRepository = shippersRepository;
		}

		[HttpGet("getshippers")]
        public IActionResult GetShippers()
        {
			var shippersEntities = _shippersRepository.GetShippers();
			var _results = new List<ShippersDto>();

			foreach(var item in shippersEntities)
			{
				_results.Add(new ShippersDto
				{
					ShipperId = item.ShipperId,
					CompanyName = item.CompanyName,
					Phone = item.Phone,
				});
			}

			return Ok(_results);
        }
    }
}