using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
	public class EmployeesController : Controller
    {
		private readonly ServiceEmployees _serviceEmployees;
		private readonly IConfiguration _configuration;
		private static List<Utilities.PictureFile> TmpPicture = new List<Utilities.PictureFile>();
		private IEnumerable<SelectListItem> regionsListItems;

		public EmployeesController(IConfiguration configuration)
		{
			_configuration = configuration;
			_serviceEmployees = new ServiceEmployees(_configuration);
		}

		// GET: Employees
		public async Task<IActionResult> Index(int page = 1, int itemsPerPage = 10)
		{
			DistributionPerPage distributionPerPage = new DistributionPerPage();

			distributionPerPage.recordCount = await _serviceEmployees.GetCount();
			distributionPerPage.itemsPerPage = itemsPerPage;
			distributionPerPage.page = page;

			distributionPerPage.CalculateDistribution();

			ViewData["PagesCount"] = int.Parse(distributionPerPage.pageCount.ToString());
			ViewData["page"] = distributionPerPage.page;
			ViewData["PageStart"] = distributionPerPage.PageStart;
			ViewData["PagingItems"] = distributionPerPage.itemsPerPage;
			ViewData["ControllerName"] = "Employees";

			var _results = await _serviceEmployees.GetEmployees(page, itemsPerPage);

			return View(_results);
		}

		// GET: Employees/Details/5
		public async Task<IActionResult> Details(int? employeeId)
        {
            if (employeeId == null)
                return NotFound();

			var _result = await _serviceEmployees.GetEmployee(employeeId.Value);

            if (_result == null)
                return NotFound();

            return View(_result);
        }

        // GET: Employees/Create
        public async Task<IActionResult> Create()
        {
			//List<Region> regionsList = await Utilities.GetRegions(_configuration);
			//ViewBag.Regions = JsonConvert.SerializeObject(regionsList);

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;
			return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Create([Bind("Id,EmployeeId,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Photo64")] Employees employees)
		public async Task<IActionResult> Create([FromForm] EmployeesForCreation employee, IFormFile imgFile)
		{
            if (ModelState.IsValid)
            {
				var newImgFile = await Utilities.ConvertPictureToBytes(imgFile);
				employee.Photo = newImgFile;

				//var new

                await _serviceEmployees.CreateEmployee(employee);
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? employeeId)
        {
            if (employeeId == null)
                return NotFound();

            var employee = await _serviceEmployees.GetEmployee(employeeId.Value);
            if (employee == null)
                return NotFound();

			TmpPicture.Add(new Utilities.PictureFile()
			{
				Id = employee.EmployeeId.ToString(),
				TmpPicture = employee.Photo
			});

			//List<Region> regionsList = await Utilities.GetRegions(_configuration);
			//ViewBag.Regions = JsonConvert.SerializeObject(regionsList);

			regionsListItems = await Utilities.FillRegionsCollection(_configuration);

			ViewData["Regions"] = regionsListItems;

			return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		//public async Task<IActionResult> Edit(int id, [Bind("Id,EmployeeId,LastName,FirstName,Title,TitleOfCourtesy,BirthDate,HireDate,Address,City,Photo64")] Employees employees)
		public async Task<IActionResult> Edit(int employeeId, [FromForm] EmployeesForUpdate employee, IFormFile imgFile)
		{
            if (employeeId != employee.EmployeeId)
                return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
					if (imgFile == null)
					{
						//employee.Photo = TmpPicture;
						employee.Photo = TmpPicture
							.Find(r => r.Id == employee.EmployeeId.ToString())
							.TmpPicture;
					}
					else
					{
						var newImgFile = await Utilities.ConvertPictureToBytes(imgFile);
						employee.Photo = newImgFile;
					}

					await _serviceEmployees.UpdateEmployee(employee);
					var TmpPic = TmpPicture
								 .Find(r => r.Id == employee.EmployeeId.ToString());

					TmpPicture.Remove(TmpPic);
				}
                catch (DbUpdateConcurrencyException)
                {
                    if (await EmployeeExists(employee.EmployeeId) == false)
                        return NotFound();
                    else
                        throw;
                }
				//return RedirectToAction(nameof(Index));
				return RedirectToAction("Details", new { employeeId = employeeId });
			}
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? employeeId)
        {
            if (employeeId == null)
                return NotFound();

            var employee = await _serviceEmployees.GetEmployee(employeeId.Value);
            if (employee == null)
                return NotFound();

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int employeeId)
        {
            await _serviceEmployees.DeleteEmployee(employeeId);

			return RedirectToAction(nameof(Index));
        }

        private async Task<bool> EmployeeExists(int employeeId)
        {
            return await _serviceEmployees.EmployeeExists(employeeId);
        }
    }
}
