using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/region")]
	public class RegionController : Controller
    {
		private readonly IRegionRepository _regionRepository;

		public RegionController(IRegionRepository regionRepository)
		{
			_regionRepository = regionRepository;
		}

		[HttpGet("getregions")]
		public IActionResult GetRegions()
        {
			var regionsEntities = _regionRepository.GetRegions();
			var _results = new List<RegionDto>();

			foreach (var item in regionsEntities)
			{
				_results.Add(new RegionDto
				{
					RegionId = item.RegionId,
					RegionDescription = item.RegionDescription
				});
			}

			return Ok(_results);
        }
    }
}