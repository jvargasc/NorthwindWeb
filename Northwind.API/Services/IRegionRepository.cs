using Northwind.API.Models;
using System.Collections.Generic;

namespace Northwind.API.Services
{
	public interface IRegionRepository
	{
		IEnumerable<Region> GetRegions();
	}
}
