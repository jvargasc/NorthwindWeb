using System;
using System.Collections.Generic;

namespace Northwind.API.Models
{
    public partial class EmployeesInfo
	{

        public int EmployeeId { get; set; }
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string Title { get; set; }
        public string TitleOfCourtesy { get; set; }

    }
}
