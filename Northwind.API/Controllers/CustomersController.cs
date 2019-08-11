using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/customers")]
	public class CustomersController : Controller
    {
		private readonly ICustomersRepository _customersRepository;

		public CustomersController(ICustomersRepository customersRepository)
		{
			_customersRepository = customersRepository;
		}

		[Route("api/getcustomers")]
		public IActionResult GetCustomers()
        {
			var customersEntities = _customersRepository.GetCustomers();
			var _results = new List<CustomersDto>();

			foreach (var item in customersEntities)
			{
				_results.Add(new CustomersDto
				{

					CustomerId = item.CustomerId,
					CompanyName = item.CompanyName,
					ContactName = item.ContactName,
					ContactTitle = item.ContactTitle,
					Address = item.Address,
					City = item.City,
					Region = item.Region,
					PostalCode = item.PostalCode,
					Country = item.Country,
					Phone = item.Phone,
					Fax = item.Fax
				});
			}

			return Ok(_results);
        }
    }
}