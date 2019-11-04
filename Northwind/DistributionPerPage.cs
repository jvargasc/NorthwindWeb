using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Northwind
{
	public class DistributionPerPage
	{
		public int page { get; set; }
		public int itemsPerPage { get; set; }
		public int recordCount { get; set; }
		public float pageCount { get; set; }
		public float decPart { get; set; }
		public int PageStart { get; set; }
		public int PagingItems { get; set; }

		public void CalculateDistribution()
		{

			pageCount = recordCount / itemsPerPage;

			decPart = recordCount % itemsPerPage;

			if (decPart > 0)
				pageCount++;

			if (recordCount > 10)
				pageCount = (int)Math.Truncate(pageCount);
			else
				pageCount = 0;

			//ordersPerPage = itemsPerPage;

			if (recordCount <= itemsPerPage)
				page = 0;

			//ViewData["page"] = page;

			if (page >= 10)
			{
				PageStart = page - 9;
				PagingItems = itemsPerPage;
			}
			else
			{
				PageStart = page;
				PagingItems = itemsPerPage + 9;
			}
		}
	}
}
