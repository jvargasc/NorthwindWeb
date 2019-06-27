using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Views.Employees
{
	public class IndexModel : PageModel
    {
		public IEnumerable<Models.Employees> Employees { get; set; }

		public void OnGet()
        {

        }
    }
}