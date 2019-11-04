using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/region")]
	public class RegionController : Controller
    {
		private readonly IRegionRepository _regionRepository;
		private readonly IMapper _mapper;

		public RegionController(IRegionRepository regionRepository,
									IMapper mapper)
		{
			_regionRepository = regionRepository;
			_mapper = mapper;
		}

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _regionRepository.GetCount());
		}

		[HttpGet("getregions")]
		public async Task<ActionResult<IEnumerable<Models.RegionDto>>> GetRegions(int page = 0, int itemsPerPage = 0)
		{
			var regionsEntities = await _regionRepository.GetRegions(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.RegionDto>>(regionsEntities);

			return Ok(_results);
        }

		[HttpGet("getregion/{regionId}")]
		public async Task<ActionResult<Models.Regions>> GetRegion(int regionId)
		{
			var regionEntity = await _regionRepository.GetRegion(regionId);
			var _result = _mapper.Map<Regions>(regionEntity);
						
			return Ok(_result);
		}

		[HttpGet("regionexists/{regionId}")]
		public async Task<ActionResult<Models.Regions>> RegionExists(int regionId)
		{
			var regionExists = await _regionRepository.RegionExits(regionId);
			
			return Ok(regionExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateRegion(
			[FromBody] Models.RegionForCreation regionToCreate)
		{
			if (regionToCreate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			regionToCreate.RegionId = await regionToCreate.GenerateNewRegionId(_regionRepository);

			var regionEntity = _mapper.Map<Regions>(regionToCreate);
			_regionRepository.AddRegion(regionEntity);

			await _regionRepository.SaveChanges();

			await _regionRepository.GetRegion(regionEntity.RegionId);

			return Ok(CreatedAtRoute("GetRegion",
						new { regionId = regionEntity.RegionId },
						_mapper.Map<Models.Regions>(regionEntity)));
		}

		[HttpPut("{regionId}")]
		public async Task<IActionResult> UpdateRegion(int regionId,
			[FromBody] Models.RegionForUpdate regionToUpdate)
		{
			if (regionToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var regionEntity = await _regionRepository.GetRegion(regionId);
			if (regionEntity == null)
				return NotFound();

			_mapper.Map(regionToUpdate, regionEntity);

			await _regionRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Regions>(regionEntity));

		}

		[HttpDelete("{regionId}")]
		public async Task<IActionResult> DeleteRegion(int regionId)
		{
			var regionEntity = await _regionRepository.GetRegion(regionId);
			if (regionEntity == null)
				return NotFound();

			_regionRepository.DeleteRegion(regionEntity);
			await _regionRepository.SaveChanges();

			return NoContent();
		}

	}
}