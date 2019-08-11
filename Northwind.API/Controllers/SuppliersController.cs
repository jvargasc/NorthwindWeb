using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/suppliers")]
    public class SuppliersController : Controller
    {
		private readonly ISuppliersRepository _suppliersRepository;

		public SuppliersController(ISuppliersRepository suppliersRepository)
		{
			_suppliersRepository = suppliersRepository;
		}

		[HttpGet("getsuppliers")]
        public IActionResult GetSuppliers()
        {
			var suppliersEntities = _suppliersRepository.GetSuppliers();
			var _results = new List<SuppliersDto>();

			foreach (var item in suppliersEntities)
			{
				_results.Add(new SuppliersDto
				{
					SupplierId = item.SupplierId,
					CompanyName = item.CompanyName,
					ContactName = item.ContactName,
					ContactTitle = item.ContactTitle,
					Address = item.Address,
					City = item.City,
					Region = item.Region,
					PostalCode = item.PostalCode,
					Country = item.Country,
					Phone = item.Phone,
					Fax = item.Fax,
					HomePage = item.HomePage
				});
			}

			return Ok(_results);
        }
    }
}