using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class ShippersController : Controller
    {
		private readonly IConfiguration _configuration;

		public ShippersController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var shippers = new ServiceShippers(_configuration);

			return View(await shippers.GetShippers());
		}
	}
}