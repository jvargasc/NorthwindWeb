using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class SuppliersController : Controller
    {
		private readonly IConfiguration _configuration;

		public SuppliersController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var suppliers = new ServiceSuppliers(_configuration);

			return View(await suppliers.GetSuppliers());
		}
	}
}