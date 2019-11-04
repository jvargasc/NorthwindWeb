using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/suppliers")]
    public class SuppliersController : Controller
    {
		private readonly ISuppliersRepository _suppliersRepository;
		private readonly IMapper _mapper;

		public SuppliersController(ISuppliersRepository suppliersRepository,
								   IMapper mapper)
		{
			_suppliersRepository = suppliersRepository;
			_mapper = mapper;
		}

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _suppliersRepository.GetCount());
		}

		[HttpGet("getsuppliers")]
        public async Task<ActionResult<IEnumerable<Models.SuppliersDto>>> GetSuppliers(int page = 0, int itemsPerPage = 0)
		{
			var suppliersEntities = await _suppliersRepository.GetSuppliers(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.SuppliersDto>>(suppliersEntities);

			return Ok(_results);
        }

		[HttpGet("getsupplier/{supplierId}")]
		public async Task<ActionResult<Suppliers>> GetSupplier(int supplierId)
		{
			var suppliersEntity = await _suppliersRepository.GetSupplier(supplierId);
			var _results = _mapper.Map<Suppliers>(suppliersEntity);

			return Ok(_results);
		}

		[HttpGet("supplierexists/{supplierId}")]
		public async Task<ActionResult<Suppliers>> SupplierExists(int supplierId)
		{
			var supplierExists = await _suppliersRepository.SupplierExits(supplierId);

			return Ok(supplierExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateSupplier(
			[FromBody] Models.SuppliersForCreation suppliersToCreate)
		{
			if (suppliersToCreate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var supplierEntity = _mapper.Map<Suppliers>(suppliersToCreate);
			_suppliersRepository.AddSupplier(supplierEntity);

			await _suppliersRepository.SaveChanges();

			await _suppliersRepository.GetSupplier(supplierEntity.SupplierId);

			return Ok( CreatedAtRoute("GetSupplier",
				new { supplierId = supplierEntity.SupplierId },
				_mapper.Map<Models.Suppliers>(supplierEntity)));
		}

		[HttpPut("{supplierId}")]
		public async Task<IActionResult> UpdateSupplier(int supplierId,
			[FromBody] Models.SuppliersForUpdate supplierToUpdate)
		{
			if (supplierToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var supplierEntity = await _suppliersRepository.GetSupplier(supplierId);
			if (supplierEntity == null)
			{
				return NotFound();
			}

			supplierToUpdate.Regions = new Models.Regions()
			{
				RegionDescription = supplierEntity.Regions.RegionDescription,
				RegionId = supplierEntity.Regions.RegionId
			};

			_mapper.Map(supplierToUpdate, supplierEntity);

			await _suppliersRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Suppliers>(supplierEntity));
		}

		[HttpDelete("{supplierId}")]
		public async Task<IActionResult> DeleteSupplier(int supplierId)
		{
			var supplierEntity = await _suppliersRepository.GetSupplier(supplierId);
			if (supplierEntity == null)
				return NotFound();

			_suppliersRepository.DeleteSupplier(supplierEntity);
			await _suppliersRepository.SaveChanges();

			return NoContent();
		}
	}
}