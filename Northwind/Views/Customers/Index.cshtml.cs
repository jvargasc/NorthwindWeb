using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Views.Customers
{
    public class IndexModel : PageModel
    {
		public IEnumerable<Models.Customers> Customers { get; set; }
		public void OnGet()
        {

        }
    }
}