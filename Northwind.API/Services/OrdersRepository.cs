using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Northwind.API.Contexts;
using Northwind.API.Entities;
using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using AutoMapper;

namespace Northwind.API.Services
{
	public class OrdersRepository : IOrdersRepository, IDisposable
	{
		private NorthwindContext _context;
		private readonly IMapper _mapper;

		public OrdersRepository(NorthwindContext context,
								IMapper mapper)
		{
			_context = context;
			_mapper = mapper;
		}

		public async Task<int> GetCount()
		{
			return await _context.Orders.CountAsync();
		}

		public async Task<IEnumerable<Orders>> GetOrders(int page = 0, int itemsPerPage = 0)
		{

			if (page == 0 || itemsPerPage == 0)
			{

				var orders = await _context.Orders
										   .Include(c => c.Customers)
										   .Include(e => e.Employees)
										   .Include(r => r.Regions)
										   .Include(s => s.Shippers)
										   .OrderBy(c => c.OrderId)
										   .Take(10)
										   .ToListAsync();

				return orders;
			}
			else
			{
				var recordCount = await _context.Orders.CountAsync();
				int pageToTake = 0;
				int pageToSkip = 0;

				Utilities.getPageSizes(recordCount, itemsPerPage, page, out pageToTake, out pageToSkip);

				return await _context.Orders
									 .Include(c => c.Customers)
									 .Include(e => e.Employees)
									 .Include(r => r.Regions)
									 .Include(s => s.Shippers)
									 .OrderBy(c => c.OrderId).Skip(pageToSkip).Take(pageToTake)
									 .ToListAsync();
			}
		}
		
		public async Task<Orders> GetOrder(int orderId)
		{
			return await _context.Orders
								.Include(c => c.Customers)
								.Include(e => e.Employees)
								.Include(r => r.Regions)
								.Include(s => s.Shippers)
								.Where(c => c.OrderId == orderId).FirstOrDefaultAsync();
											//.Include(od => od.OrderDetails)
		}

		public async Task<List<OrderDetails>> GetOrderDetails(int orderId)
		{
			return await _context.OrderDetails
								 .Include(p => p.Products)
								 .Where(od => od.OrderId == orderId).ToListAsync();
		}

		public async void AddOrder(Orders orderToAdd)
		{
			if (orderToAdd == null)
			{
				throw new ArgumentNullException(nameof(orderToAdd));
			}

			await _context.AddAsync(orderToAdd);
		}

		public async Task<bool> SaveChanges()
		{
			return (await _context.SaveChangesAsync() > 0);
		}

		public async Task<bool> OrderExits(int orderId)
		{
			return (await _context.Orders.AnyAsync(e => e.OrderId == orderId));
		}

		public void DeleteOrder(Orders orderToDelete)
		{
			if (orderToDelete == null)
			{
				throw new ArgumentNullException(nameof(orderToDelete));
			}

			_context.Remove(orderToDelete);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (_context != null)
				{
					_context.Dispose();
					_context = null;
				}
			}
		}

		public async Task<IEnumerable> OrderDetails()
		{
			//var orders = await _context.Orders.ToListAsync();
			//var employees = await _context.Employees.ToListAsync();
			//var customers = await _context.Customers.ToListAsync();

			var ordersList = (from order in (await _context.Orders.ToListAsync())
							  join employee in ( await _context.Employees.ToListAsync() )on order.EmployeeId equals employee.EmployeeId
							  join customer in ( await _context.Customers.ToListAsync() )on order.CustomerId equals customer.CustomerId
							  select new
							  {
								  order.CustomerId,
								  customer.CompanyName,
								  customer.ContactName,
								  customer.ContactTitle,
								  order.EmployeeId,
								  employee.FirstName,
								  employee.LastName,
								  order.Freight,
								  order.OrderDate,
								  order.OrderId,
								  order.RequiredDate,
								  order.ShipAddress,
								  order.ShipCity,
								  order.ShipCountry,
								  order.ShipName,
								  order.ShippedDate,
								  order.ShipPostalCode,
								  order.RegionId,
								  order.ShipperId
							  });

			return ordersList;

			//return orders;
		}

	}
}
