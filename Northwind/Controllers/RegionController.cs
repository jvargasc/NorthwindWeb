using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class RegionController : Controller
    {
		private readonly IConfiguration _configuration;

		public RegionController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var regions = new ServiceRegion(_configuration);

			return View(await regions.GetRegions());
		}
	}
}