﻿using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class ShippersForUpdate
    {
        //public Shippers()
        //{
        //    Orders = new HashSet<Orders>();
        //}

        public int ShipperId { get; set; }
        public string CompanyName { get; set; }
        public string Phone { get; set; }

        //public ICollection<Orders> Orders { get; set; }
    }
}
