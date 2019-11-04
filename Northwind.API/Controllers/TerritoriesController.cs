using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/territories")]
	public class TerritoriesController : Controller
	{
		private readonly ITerritoriesRepository _territoriesRepository;
		private readonly IMapper _mapper;

		public TerritoriesController(ITerritoriesRepository territoriesRepository,
									IMapper mapper)
		{
			_territoriesRepository = territoriesRepository;
			_mapper = mapper;
		}

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _territoriesRepository.GetCount());
		}

		[HttpGet("getterritories")]
		public async Task<ActionResult<IEnumerable<Models.TerritoriesDto>>> GetTerritories(int page = 0, int itemsPerPage = 0)
		{
			var territoriesEntities = await _territoriesRepository.GetTerritories(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.TerritoriesDto>>(territoriesEntities);

			return Ok(_results);
		}

		[HttpGet("getterritory/{territoryId}")]
		public async Task<ActionResult<Models.Territories>> GetTerritory(string territoryId)
		{
			var territoryEntity = await _territoriesRepository.GetTerritory(territoryId);
			var _result = _mapper.Map<Territories>(territoryEntity);

			return Ok(_result);
		}

		[HttpGet("territoryexists/{territoryId}")]
		public async Task<ActionResult<bool>> TerritoryExists(string territoryId)
		{
			var territoryExists = await _territoriesRepository.TerritoryExits(territoryId);
			
			return Ok(territoryExists);
		}
		
		[HttpPost]
		public async Task<IActionResult> CreateTerritory(
			[FromBody] Models.TerritoriesForCreation territoryToCreate)
		{
			if (territoryToCreate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			territoryToCreate.TerritoryId = await territoryToCreate.GenerateNewTerritoryId(_territoriesRepository);

			var territoryEntity = _mapper.Map<Territories>(territoryToCreate);
			_territoriesRepository.AddTerritory(territoryEntity);

			await _territoriesRepository.SaveChanges();

			await _territoriesRepository.GetTerritory(territoryEntity.TerritoryId);

			return Ok(CreatedAtRoute("GetTerritory",
				new { territoryId = territoryEntity.TerritoryId },
				_mapper.Map<Models.Territories>(territoryEntity)));

		}

		[HttpPut("{territoryId}")]
		public async Task<IActionResult> UpdateTerritory(string territoryId,
			[FromBody] Models.TerritoriesForUpdate territoryToUpdate)
		{
			if (territoryToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var territoryEntity = await _territoriesRepository.GetTerritory(territoryId);
			if (territoryEntity == null)
				return NotFound();

			territoryToUpdate.Regions = new Models.Regions()
			{
				RegionDescription = territoryEntity.Regions.RegionDescription,
				RegionId = territoryEntity.Regions.RegionId
			};				

			_mapper.Map(territoryToUpdate, territoryEntity);

			await _territoriesRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Territories>(territoryEntity));
		}

		[HttpDelete("{territoryId}")]
		public async Task<IActionResult> DeleteTerritory(string territoryId)
		{
			var territoryEntity = await _territoriesRepository.GetTerritory(territoryId);
			if (territoryEntity == null)
				return NotFound();

			_territoriesRepository.DeleteTerritory(territoryEntity);
			await _territoriesRepository.SaveChanges();

			return NoContent();
		}

	}
}