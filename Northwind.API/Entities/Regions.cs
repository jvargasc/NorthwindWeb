using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class Regions
    {
		[Key]
        public int RegionId { get; set; }
		[Required, MaxLength(50)]
		public string RegionDescription { get; set; }
		        
    }
}
