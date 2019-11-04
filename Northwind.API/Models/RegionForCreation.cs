using Northwind.API.Services;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Northwind.API.Models
{
    public partial class RegionForCreation
    {
        //public Region()
        //{
        //    Territories = new HashSet<Territories>();
        //}

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

		//public ICollection<Territories> Territories { get; set; }

		public async Task<int> GenerateNewRegionId(IRegionRepository _regionRepository)
		{
			return await _regionRepository.GetNewId();
		}

	}
}
