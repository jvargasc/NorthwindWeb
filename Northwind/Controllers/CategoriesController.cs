using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class CategoriesController : Controller
    {
		private readonly IConfiguration _configuration;
		public CategoriesController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var categories = new ServiceCategories(_configuration);

			return View(await categories.GetCategories());
		}
	}
}