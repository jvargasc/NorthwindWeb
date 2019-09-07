using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
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

		[HttpGet("getterritories")]
		public async Task<ActionResult> GetTerritories()
		{
			var territoriesEntities = await _territoriesRepository.GetTerritories();
			var _results = _mapper.Map<IEnumerable<TerritoriesDto>>(territoriesEntities);
			//var _results = new List<TerritoriesDto>();

			//foreach(var item in territoriesEntities)
			//{
			//	_results.Add(new TerritoriesDto
			//	{
			//		TerritoryId = item.TerritoryId,
			//		TerritoryDescription = item.TerritoryDescription,
			//		RegionId = item.RegionId,
			//		Region = item.Region
			//	});
			//}

			return Ok(_results);
		}

		[HttpGet("getterritory/{territoryId}")]
		public async Task<ActionResult> GetTerritory(string territoryId)
		{
			var territoryEntity = await _territoriesRepository.GetTerritory(territoryId);
			var _result = _mapper.Map<Territories>(territoryEntity);

			return Ok(_result);
		}
	}
}