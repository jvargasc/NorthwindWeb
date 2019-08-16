using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Views.Categories
{
	public class IndexModel : PageModel
    {
		public IEnumerable<Models.Categories> Categories { get; set; }
		public void OnGet()
        {
        }
    }
}