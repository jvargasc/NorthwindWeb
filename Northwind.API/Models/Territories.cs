using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class Territories
    {
		//public Territories()
		//{
		//	Regions = new Regions();
		//	//    EmployeeTerritories = new HashSet<EmployeeTerritories>();
		//}

		public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

		public Regions Regions { get; set; }
		//public ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
	}
}
