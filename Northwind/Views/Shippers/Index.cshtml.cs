﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Northwind.Views.Shippers
{
    public class IndexModel : PageModel
    {
		public IEnumerable<Models.Shippers> Shippers { get; set; }
		public void OnGet()
        {

        }
    }
}