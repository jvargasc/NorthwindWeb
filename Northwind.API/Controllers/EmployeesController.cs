using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/employees")]
	public class EmployeesController : Controller
    {
		private IEmployeesRepository _employeesRepository;

		public EmployeesController(IEmployeesRepository employeesRepository)
		{
			_employeesRepository = employeesRepository;
		}

		[HttpGet("getemployees")]
		public IActionResult GetEmployees()
		{
			//var results = _employeesRepository.GetEmployees();
			var employeesEntities = _employeesRepository.GetEmployees();
			var _results = new List<EmployeesDto>();

			foreach (var item in employeesEntities)
			{
				_results.Add(new EmployeesDto
				{
					EmployeeId = item.EmployeeId,
					FirstName = item.FirstName,
					LastName = item.LastName,
					Address = item.Address,
					BirthDate= item.BirthDate,
					City = item.City,
					HireDate = item.HireDate,
					Title = item.Title,
					TitleOfCourtesy = item.TitleOfCourtesy
				});
			}			

			///var results = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDto>>(cityEntities);

			return Ok(_results);
		}
	}
}