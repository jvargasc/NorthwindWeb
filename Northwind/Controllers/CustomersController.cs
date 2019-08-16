using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class CustomersController : Controller
    {
		private readonly IConfiguration _configuration;

		public CustomersController(IConfiguration configuration)
		{
			_configuration = configuration;
		}

		public async Task<IActionResult> Index()
		{
			var customers = new ServiceCustomers(_configuration);

			return View(await customers.GetCustomers());
		}
	}
}