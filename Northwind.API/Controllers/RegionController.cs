using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
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

		[HttpGet("getregions")]
		public async Task<ActionResult> GetRegions()
        {
			var regionsEntities = await _regionRepository.GetRegions();
			var _results = _mapper.Map<IEnumerable<RegionDto>>(regionsEntities);

			//var _results = new List<RegionDto>();

			//foreach (var item in regionsEntities)
			//{
			//	_results.Add(new RegionDto
			//	{
			//		RegionId = item.RegionId,
			//		RegionDescription = item.RegionDescription
			//	});
			//}

			return Ok(_results);
        }

		[HttpGet("getregion/{regionId}")]
		public async Task<ActionResult> GetRegion(int regionId)
		{
			var regionEntity = await _regionRepository.GetRegion(regionId);
			var _result = _mapper.Map<Region>(regionEntity);
						
			return Ok(_result);
		}

	}
}