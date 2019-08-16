using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class OrdersController : Controller
    {
		private readonly IConfiguration _configuration;

		public OrdersController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var orders = new ServiceOrders(_configuration);

			return View(await orders.GetOrders());
		}
	}
}