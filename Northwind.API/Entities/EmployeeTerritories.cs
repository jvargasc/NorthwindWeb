﻿using System;
using System.Collections.Generic;

namespace Northwind.API.Entities
{
    public partial class EmployeeTerritories
    {
        public int EmployeeId { get; set; }
        public string TerritoryId { get; set; }

        //public Employees Employee { get; set; }
        //public Territories Territory { get; set; }
    }
}
