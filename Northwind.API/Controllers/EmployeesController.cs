using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Models;
using Northwind.API.Services;

namespace Northwind.API.Controllers
{
	[Route("api/employees")]
	public class EmployeesController : Controller
    {
		private IEmployeesRepository _employeesRepository;
		private readonly IMapper _mapper;

		public EmployeesController(IEmployeesRepository employeesRepository,
									IMapper mapper)
		{
			_employeesRepository = employeesRepository;
			_mapper = mapper;
		}

		[HttpGet("getemployees")]
		public async Task<ActionResult> GetEmployees()
		{
			var employeesEntities = await _employeesRepository.GetEmployees();
			var _results = _mapper.Map<IEnumerable<EmployeesDto>>(employeesEntities);
					
			return Ok(_results);
		}

		[HttpGet("getemployee/{employeeId}")]
		public async Task<ActionResult> GetEmployee(int employeeId)
		{
			var employeeEntity = await _employeesRepository.GetEmployee(employeeId);
			var _result = _mapper.Map<Employees>(employeeEntity);

			return Ok(_result);
		}
	}
}