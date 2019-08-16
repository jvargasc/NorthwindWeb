using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class ProductsController : Controller
    {
		private readonly IConfiguration _configuration;

		public ProductsController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var products = new ServiceProducts(_configuration);

			return View(await products.GetProducts());
		}
	}
}