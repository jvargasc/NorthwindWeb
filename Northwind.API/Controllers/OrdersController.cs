using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/orders")]
	public class OrdersController : Controller
    {
		private readonly IOrdersRepository _ordersRepository;

		public OrdersController(IOrdersRepository ordersRepository)
		{
			_ordersRepository = ordersRepository;
		}

		[HttpGet("getorders")]
		public IActionResult GetOrders()
        {
			var ordersEntities = _ordersRepository.GetOrders();
			var _results = new List<OrdersDto>();

			foreach(var item in ordersEntities)
			{
				_results.Add(new OrdersDto
				{
					OrderId = item.OrderId,
					CustomerId = item.CustomerId,
					EmployeeId = item.EmployeeId,
					OrderDate = item.OrderDate,
					RequiredDate = item.RequiredDate,
					ShippedDate = item.ShippedDate,
					ShipVia = item.ShipVia,
					Freight = item.Freight,
					ShipName = item.ShipName,
					ShipAddress = item.ShipAddress,
					ShipCity = item.ShipCity,
					ShipRegion = item.ShipRegion,
					ShipPostalCode = item.ShipPostalCode,
					ShipCountry = item.ShipCountry,
					Customer = item.Customer,
					Employee = item.Employee,
					ShipViaNavigation = item.ShipViaNavigation
				});
			}

            return Ok(_results);
        }
    }
}