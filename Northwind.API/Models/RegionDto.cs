﻿using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class RegionDto
    {
        public RegionDto()
        {
            //Territories = new HashSet<Territories>();
        }

        public int RegionId { get; set; }
        public string RegionDescription { get; set; }

        //public ICollection<Territories> Territories { get; set; }
    }
}
