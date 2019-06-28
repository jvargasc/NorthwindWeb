using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class EmployeesController : Controller
    {
		public IConfiguration Configuration { get; }
		public EmployeesController(IConfiguration configuration)
		{
			Configuration = configuration;
		}
		
		public async Task<IActionResult> Index()
        {
			var employees = new ServiceEmployees(Configuration);

			return View( await employees.GetEmployees() );
        }
    }
}