using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class TerritoriesDto
    {
        public TerritoriesDto()
        {
            //EmployeeTerritories = new HashSet<EmployeeTerritories>();
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public Region Region { get; set; }
        //public ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }
    }
}
