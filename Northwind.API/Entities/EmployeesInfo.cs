using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Northwind.API.Entities
{
    public partial class EmployeesInfo
	{
		[Key]
		public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }

    }
}
