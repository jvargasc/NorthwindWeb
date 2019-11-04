using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
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

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _shippersRepository.GetCount());
		}

		[HttpGet("getshippers")]
        public async Task<ActionResult<IEnumerable<Models.ShippersDto>>> GetShippers(int page = 0, int itemsPerPage = 0)
		{
			var shippersEntities = await _shippersRepository.GetShippers(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.ShippersDto>>(shippersEntities);

			return Ok(_results);
        }

		[HttpGet("getshipper/{shipperId}")]
		public async Task<ActionResult<Models.Shippers>> GetShipper(int shipperId)
		{
			var shipperEntity = await _shippersRepository.GetShipper(shipperId);
			var _result = _mapper.Map<Shippers>(shipperEntity);

			return Ok(_result);
		}

		[HttpGet("shipperexists/{shipperId}")]
		public async Task<ActionResult<bool>> ShipperExists(int shipperId)
		{
			var shipperExists = await _shippersRepository.ShipperExits(shipperId);

			return Ok(shipperExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateShipper(
			[FromBody] Models.ShippersForCreation shipperToCreate)
		{
			if (shipperToCreate == null)
				return NotFound();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var shipperEntity = _mapper.Map<Shippers>(shipperToCreate);
			_shippersRepository.AddShipper(shipperEntity);

			await _shippersRepository.SaveChanges();

			await _shippersRepository.GetShipper(shipperToCreate.ShipperId);

			return Ok(CreatedAtRoute("GetRegion",
						new { shipperId = shipperToCreate.ShipperId },
						_mapper.Map<Models.Shippers>(shipperEntity)));

		}

		[HttpPut("{shipperId}")]
		public async Task<IActionResult> UpdateShipper(int shipperId,
			[FromBody] Models.ShippersForUpdate shipperToUpdate)
		{
			if (shipperToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var shipperEntity = await _shippersRepository.GetShipper(shipperId);
			if (shipperEntity == null)
				return NotFound();

			_mapper.Map(shipperToUpdate, shipperEntity);

			await _shippersRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Shippers>(shipperEntity));

		}

		[HttpDelete("{shipperId}")]
		public async Task<IActionResult> DeleteShipper(int shipperId)
		{
			var shipperEntity = await _shippersRepository.GetShipper(shipperId);
			if (shipperEntity == null)
				return NotFound();

			_shippersRepository.DeleteShipper(shipperEntity);
			await _shippersRepository.SaveChanges();

			return NoContent();
		}
	}
}