using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Northwind.Models;

namespace Northwind.Models
{
    public class Context : DbContext
    {
        public Context (DbContextOptions<Context> options)
            : base(options)
        {
        }

        public DbSet<Northwind.Models.Categories> Categories { get; set; }

        public DbSet<Northwind.Models.Customers> Customers { get; set; }

        public DbSet<Northwind.Models.Employees> Employees { get; set; }

        public DbSet<Northwind.Models.Orders> Orders { get; set; }

        public DbSet<Northwind.Models.Products> Products { get; set; }

        public DbSet<Northwind.Models.Region> Region { get; set; }

        public DbSet<Northwind.Models.Shippers> Shippers { get; set; }

        public DbSet<Northwind.Models.Suppliers> Suppliers { get; set; }

        public DbSet<Northwind.Models.Territories> Territories { get; set; }
    }
}
