using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
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

		[HttpGet("getorders")]
		public async Task<ActionResult> GetOrders()
		{
			var ordersEntities = await _ordersRepository.GetOrders();
			var _results = _mapper.Map<IEnumerable<OrdersDto>>(ordersEntities);

			//var _results = new List<OrdersDto>();

			//foreach(var item in ordersEntities)
			//{
			//	_results.Add(new OrdersDto
			//	{
			//		OrderId = item.OrderId,
			//		CustomerId = item.CustomerId,
			//		EmployeeId = item.EmployeeId,
			//		OrderDate = item.OrderDate,
			//		RequiredDate = item.RequiredDate,
			//		ShippedDate = item.ShippedDate,
			//		ShipVia = item.ShipVia,
			//		Freight = item.Freight,
			//		ShipName = item.ShipName,
			//		ShipAddress = item.ShipAddress,
			//		ShipCity = item.ShipCity,
			//		ShipRegion = item.ShipRegion,
			//		ShipPostalCode = item.ShipPostalCode,
			//		ShipCountry = item.ShipCountry,
			//		Customer = item.Customer,
			//		Employee = item.Employee,
			//		ShipViaNavigation = item.ShipViaNavigation
			//	});
			//}

			return Ok(_results);
		}

		[HttpGet("getorder/{orderId}")]
		public async Task<ActionResult> GetOrder(int orderId)
		{
			var orderEntity = await _ordersRepository.GetOrder(orderId);
			var _result = _mapper.Map<OrdersDto>(orderEntity);

			return Ok(_result);
		}
	}
}