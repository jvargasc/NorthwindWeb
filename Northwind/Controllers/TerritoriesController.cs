using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class TerritoriesController : Controller
    {
		private readonly IConfiguration _configuration;

		public TerritoriesController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var Territories = new ServiceTerritories(_configuration);

			return View(await Territories.GetTerritories());
		}
	}
}