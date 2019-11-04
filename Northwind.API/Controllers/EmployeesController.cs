using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Northwind.API.Entities;
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

		[HttpGet("getcount")]
		public async Task<ActionResult<int>> GetCount()
		{
			return Ok(await _employeesRepository.GetCount());
		}

		[HttpGet("getemployees")]
		public async Task<ActionResult<IEnumerable<Models.EmployeesDto>>> GetEmployees(int page = 0, int itemsPerPage = 0)
		{
			var employeesEntities = await _employeesRepository.GetEmployees(page, itemsPerPage);
			var _results = _mapper.Map<IEnumerable<Models.EmployeesDto>>(employeesEntities);
					
			return Ok(_results);
		}

		[HttpGet("getemployee/{employeeId}")]
		public async Task<ActionResult<Models.EmployeesDto>> GetEmployee(int employeeId)
		{
			var employeeEntity = await _employeesRepository.GetEmployee(employeeId);
			var _result = _mapper.Map<Employees>(employeeEntity);

			return Ok(_result);
		}

		[HttpGet("employeeexists/{employeeId}")]
		public async Task<ActionResult<bool>> EmployeeExists(int employeeId)
		{
			var employeeExitst = await _employeesRepository.EmployeeExits(employeeId);

			return Ok(employeeExitst);
		}

		[HttpPost]
		public async Task<IActionResult> CreateEmployee(
			[FromBody] Models.EmployeesForCreation employeesForCreation)
		{
			if (employeesForCreation == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return new UnprocessableEntityObjectResult(ModelState);
			}

			var employeeEntity = _mapper.Map<Employees>(employeesForCreation);
			_employeesRepository.AddEmployee(employeeEntity);

			await _employeesRepository.SaveChanges();

			await _employeesRepository.GetEmployee(employeeEntity.EmployeeId);

			return Ok(CreatedAtRoute("GetEmployee",
					new { employeeId = employeeEntity.EmployeeId },
					_mapper.Map<Models.Employees>(employeeEntity)));
		}

		[HttpPut("{employeeId}")]
		public async Task<IActionResult> UpdateEmployee(int employeeId,
			[FromBody] Models.EmployeesForUpdate employeesForUpdate)
		{
			if (employeesForUpdate == null)
			{
				return BadRequest();
			}

			if (!ModelState.IsValid)
			{
				return new UnprocessableEntityObjectResult(ModelState);
			}

			var employeeEntity = await _employeesRepository.GetEmployee(employeeId);
			if (employeeEntity == null)
			{
				return NotFound();
			}

			_mapper.Map(employeesForUpdate.Regions, employeeEntity.Regions);
			_mapper.Map(employeesForUpdate, employeeEntity);

			await _employeesRepository.SaveChanges();

			return Ok(_mapper.Map<Models.Employees>(employeeEntity));
		}

		[HttpDelete("{employeeId}")]
		public async Task<IActionResult> DeleteEmployee(int employeeId)
		{
			var employeeEntity = await _employeesRepository.GetEmployee(employeeId);
			if (employeeEntity == null)
			{
				return NotFound();
			}

			_employeesRepository.DeleteEmployee(employeeEntity);
			await _employeesRepository.SaveChanges();

			return NoContent();
		}

	}
}