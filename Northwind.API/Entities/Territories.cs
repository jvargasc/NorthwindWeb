using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class Territories
    {
		public Territories()
		{
			Regions = new Regions();
		}

		[Key, MaxLength(20)]
        public string TerritoryId { get; set; }
		[Required, MaxLength(50)]
        public string TerritoryDescription { get; set; }
		public int RegionId { get; set; }

		public Regions Regions { get; set; }

	}
}
