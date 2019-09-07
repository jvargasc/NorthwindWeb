using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/shippers")]
	public class ShippersController : Controller
    {
		private readonly IShippersRepository _shippersRepository;
		private readonly IMapper _mapper;

		public ShippersController(IShippersRepository shippersRepository,
									IMapper mapper)
		{
			_shippersRepository = shippersRepository;
			_mapper = mapper;
		}

		[HttpGet("getshippers")]
        public async Task<ActionResult> GetShippers()
        {
			var shippersEntities = await _shippersRepository.GetShippers();
			var _results = _mapper.Map<IEnumerable<ShippersDto>>(shippersEntities);
			//var _results = new List<ShippersDto>();

			//foreach(var item in shippersEntities)
			//{
			//	_results.Add(new ShippersDto
			//	{
			//		ShipperId = item.ShipperId,
			//		CompanyName = item.CompanyName,
			//		Phone = item.Phone,
			//	});
			//}

			return Ok(_results);
        }

		[HttpGet("getshipper/{shipperId}")]
		public async Task<ActionResult> GetShipper(int shipperId)
		{
			var shipperEntity = await _shippersRepository.GetShipper(shipperId);
			var _result = _mapper.Map<Shippers>(shipperEntity);

			return Ok(_result);
		}
	}
}