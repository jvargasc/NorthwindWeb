using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/orders")]
	public class OrdersController : Controller
	{
		private readonly IOrdersRepository _ordersRepository;
		private readonly IMapper _mapper;

		public OrdersController(IOrdersRepository ordersRepository,
									IMapper mapper)
		{
			_ordersRepository = ordersRepository;
			_mapper = mapper;
		}

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _ordersRepository.GetCount());
		}

		[HttpGet("getorders")]
		public async Task<ActionResult<IEnumerable<Models.Orders>>> GetOrders(int page = 0, int itemsPerPage = 0)
		{
			var ordersEntities = await _ordersRepository.GetOrders(page, itemsPerPage);

			var _results = _mapper.Map<IEnumerable<Models.Orders>>(ordersEntities);

			return Ok(_results);
		}

		[HttpGet("getorder/{orderId}")]
		public async Task<ActionResult<Models.Orders>> GetOrder(int orderId)
		{
			var orderEntity = await _ordersRepository.GetOrder(orderId);
			orderEntity.OrderDetails = await _ordersRepository.GetOrderDetails(orderId);

			var _result = _mapper.Map<Models.OrdersDto>(orderEntity);

			return Ok(_result);
		}
		
		[HttpGet("orderexists/{orderId}")]
		public async Task<ActionResult<bool>> OrderExists(int orderId)
		{
			var orderExists = await _ordersRepository.OrderExits(orderId);

			return Ok(orderExists);
		}

		[HttpPost]
		public async Task<IActionResult> CreateOrder(
			[FromBody] Models.OrdersForCreation orderToCreate)
		{
			if (orderToCreate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var orderEntity = _mapper.Map<Orders>(orderToCreate);
			_ordersRepository.AddOrder(orderEntity);

			await _ordersRepository.SaveChanges();

			await _ordersRepository.GetOrder(orderEntity.OrderId);

			return Ok(CreatedAtRoute("GetOrder",
						new { orderId = orderEntity.OrderId },
						_mapper.Map<Models.Orders>(orderEntity)));
		}

		[HttpPut("{orderId}")]
		public async Task<IActionResult> UpdateOrder(int orderId,
			[FromBody] Models.OrdersForUpdate ordersToUpdate)
		{
			if (ordersToUpdate == null)
				return BadRequest();

			if (!ModelState.IsValid)
				return new UnprocessableEntityObjectResult(ModelState);

			var orderEntity = await _ordersRepository.GetOrder(orderId);
			if (orderEntity == null)
				return NotFound();

			var orderDetailsEntity = await _ordersRepository.GetOrderDetails(orderId);
			if (orderDetailsEntity == null)
				return NotFound();

			ordersToUpdate.Customers = new Models.Customers()
			{
				Address = orderEntity.Customers.Address,
				City = orderEntity.Customers.City,
				CompanyName = orderEntity.Customers.CompanyName,
				ContactName = orderEntity.Customers.ContactName,
				ContactTitle = orderEntity.Customers.ContactTitle,
				Country = orderEntity.Customers.Country,
				CustomerId = orderEntity.Customers.CustomerId,
				Fax = orderEntity.Customers.Fax,
				Phone = orderEntity.Customers.Phone,
				PostalCode = orderEntity.Customers.PostalCode,
				RegionId = orderEntity.Customers.RegionId,
				Regions = new Models.Regions()
				{
					RegionDescription = orderEntity.Customers.Regions.RegionDescription,
					RegionId = orderEntity.Customers.Regions.RegionId
				}
			};

			ordersToUpdate.Employees = new Models.Employees()
			{
				Address = orderEntity.Employees.Address,
				BirthDate = orderEntity.Employees.BirthDate,
				City = orderEntity.Employees.City,
				Country = orderEntity.Employees.Country,
				EmployeeId = orderEntity.Employees.EmployeeId,
				Extension = orderEntity.Employees.Extension,
				FirstName = orderEntity.Employees.FirstName,
				HireDate = orderEntity.Employees.HireDate,
				HomePhone = orderEntity.Employees.HomePhone,
				LastName = orderEntity.Employees.LastName,
				Notes = orderEntity.Employees.Notes,
				Photo = orderEntity.Employees.Photo,
				PhotoPath = orderEntity.Employees.PhotoPath,
				PostalCode = orderEntity.Employees.PostalCode,
				RegionId = orderEntity.Employees.RegionId,
				Regions = new Models.Regions()
				{
					RegionDescription = orderEntity.Employees.Regions.RegionDescription,
					RegionId = orderEntity.Employees.Regions.RegionId,
				},
				ReportsTo = orderEntity.Employees.ReportsTo,
				Title = orderEntity.Employees.Title,
				TitleOfCourtesy = orderEntity.Employees.TitleOfCourtesy
			};

			ordersToUpdate.Regions = new Models.Regions()
			{
				RegionDescription = orderEntity.Regions.RegionDescription,
				RegionId = orderEntity.Regions.RegionId
			};

			ordersToUpdate.Shippers = new Models.Shippers()
			{
				CompanyName = orderEntity.Shippers.CompanyName,
				Phone = orderEntity.Shippers.Phone,
				ShipperId = orderEntity.Shippers.ShipperId
			};

			_mapper.Map(ordersToUpdate, orderEntity);

			await _ordersRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Orders>(orderEntity));
		}

		[HttpDelete("{orderId}")]
		public async Task<IActionResult> DeleteOrder(int orderId)
		{
			var orderEntity = await _ordersRepository.GetOrder(orderId);
			if (orderEntity == null)
				return NoContent();

			orderEntity.OrderDetails = await _ordersRepository.GetOrderDetails(orderId);		

			_ordersRepository.DeleteOrder(orderEntity);
			await _ordersRepository.SaveChanges();

			return NoContent();
		}

		//[HttpGet("orderdetails")]
		//public async Task<IActionResult> OrderDetails()
		//{
		//	var ordersEntities = await _ordersRepository.OrderDetails();
		//	//var _results = _mapper.Map<IEnumerable<Models.EmployeesDto>>(employeesEntities);

		//	//return Ok(_results);
		//	return Ok(ordersEntities);
		//}

	}
}