﻿using Northwind.API.Models;
using System.Collections.Generic;

namespace Northwind.API.Services
{
	public interface IOrdersRepository
	{
		IEnumerable<Orders> GetOrders();
	}
}