using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Northwind.API.Services;

namespace Northwind.API.Models
{
    public partial class TerritoriesForCreation
    {
        //public Territories()
        //{
        //    EmployeeTerritories = new HashSet<EmployeeTerritories>();
        //}

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

		public Regions Regions { get; set; }
		//public ICollection<EmployeeTerritories> EmployeeTerritories { get; set; }

		public async Task<string> GenerateNewTerritoryId(ITerritoriesRepository _territoriesRepository)
		{
			return await _territoriesRepository.GetNewId();
		}
	}
}
