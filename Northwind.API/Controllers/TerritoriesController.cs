using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/territories")]
    public class TerritoriesController : Controller
    {
		private readonly ITerritoriesRepository _territoriesRepository;

		public TerritoriesController(ITerritoriesRepository territoriesRepository )
		{
			_territoriesRepository = territoriesRepository;
		}

	   [HttpGet("getterritories")]
       public IActionResult GetTerritories()
       {
			var territoriesEntities = _territoriesRepository.GetTerritories();
			var _results = new List<TerritoriesDto>();

			foreach(var item in territoriesEntities)
			{
				_results.Add(new TerritoriesDto
				{
					TerritoryId = item.TerritoryId,
					TerritoryDescription = item.TerritoryDescription,
					RegionId = item.RegionId,
					Region = item.Region
				});
			}

			return Ok(_results);
       }
    }
}