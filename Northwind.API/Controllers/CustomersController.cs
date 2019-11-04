using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
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

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _customersRepository.GetCount());
		}

		[HttpGet("getcustomers")]
		public async Task<ActionResult<IEnumerable<Models.CustomersDto>>> GetCustomers(int page = 0, int itemsPerPage = 0)
		{
			var customersEntities = await _customersRepository.GetCustomers(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.CustomersDto>>(customersEntities);

			return Ok(_results);
        }

		[HttpGet("getcustomer/{customerId}")]
		public async Task<ActionResult<Models.Customers>> GetCustomer(string customerId)
		{
			var customerEntity = await _customersRepository.GetCustomer(customerId);
			var _result = _mapper.Map<Customers>(customerEntity);

			return Ok(_result);
		}

		[HttpGet("customerexists/{customerId}")]
		public async Task<ActionResult<bool>> CustomerExists(string customerId)
		{
			var customerExists = await _customersRepository.CustomerExits(customerId);

			return Ok(customerExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateCustomer(
			[FromBody] Models.CustomersForCreation customersForCreation)
		{
			if (customersForCreation == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return new UnprocessableEntityObjectResult(ModelState);
			}

			customersForCreation.GenerateCustomerId();

			var customerEntity = _mapper.Map<Customers>(customersForCreation);
			_customersRepository.AddCustomer(customerEntity);

			await _customersRepository.SaveChanges();

			await _customersRepository.GetCustomer(customerEntity.CustomerId);

			return Ok(CreatedAtRoute("GetCustomer",
					new { customerId = customerEntity.CustomerId },
					_mapper.Map<Models.Customers>(customerEntity)));
		}

		[HttpPut("{customerId}")]
		public async Task<IActionResult> UpdateCustomer(string customerId,
			[FromBody] Models.CustomersForUpdate customersToUpdate)
		{
			if (customersToUpdate == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return new UnprocessableEntityObjectResult(ModelState);
			}

			var customerEntity = await _customersRepository.GetCustomer(customerId);
			if (customerEntity == null)
			{
				return NotFound();
			}

			_mapper.Map(customersToUpdate.Regions, customerEntity.Regions);
			_mapper.Map(customersToUpdate, customerEntity);

			await _customersRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Customers>(customerEntity));
		}

		[HttpDelete("{customerId}")]
		public async Task<IActionResult> DeleteCustomer(string customerId)
		{
			var customerEntity = await _customersRepository.GetCustomer(customerId);
			if (customerEntity == null)
			{
				return NotFound();
			}

			_customersRepository.DeleteCustomer(customerEntity);
			await _customersRepository.SaveChanges();

			return NoContent();
		}
	}
}