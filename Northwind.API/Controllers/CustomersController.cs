using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/customers")]
	public class CustomersController : Controller
    {
		private readonly ICustomersRepository _customersRepository;
		private readonly IMapper _mapper;

		public CustomersController(ICustomersRepository customersRepository,
									IMapper mapper)
		{
			_customersRepository = customersRepository;
			_mapper = mapper;
		}

		[HttpGet("getcustomers")]
		public async Task<ActionResult<IEnumerable<CustomersDto>>> GetCustomers()
        {
			var customersEntities = await _customersRepository.GetCustomers();
			var _results = _mapper.Map<IEnumerable<CustomersDto>>(customersEntities);

			return Ok(_results);
        }

		[HttpGet("getcustomer/{customerId}")]
		public async Task<ActionResult<Customers>> GetCustomer(string customerId)
		{
			var customerEntity = await _customersRepository.GetCustomer(customerId);
			var _result = _mapper.Map<Customers>(customerEntity);

			return Ok(_result);
		}
	}
}