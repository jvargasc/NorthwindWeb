using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Northwind.Models;
using Northwind.Services;

namespace Northwind.Controllers
{
    public class TerritoriesController : Controller
    {
        private readonly Context _context;
		private readonly IConfiguration _configuration;

		public TerritoriesController(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        // GET: Territories
        public async Task<IActionResult> Index()
        {
			var territories = new ServiceTerritories(_configuration);
			var _results = await territories.GetTerritories();

			return View(_results);
        }

        // GET: Territories/Details/5
        public async Task<IActionResult> Details(string id)
        {
			if (string.IsNullOrEmpty(id))
			{
                return NotFound();
            }

			var territories = new ServiceTerritories(_configuration);
			var _result = await territories.GetTerritory(id);

			if (_result == null)
            {
                return NotFound();
            }

            return View(_result);
        }

        // GET: Territories/Create
        public IActionResult Create()
        {
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id");
            return View();
        }

        // POST: Territories/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TerritoryId,TerritoryDescription,RegionId")] Territories territories)
        {
            if (ModelState.IsValid)
            {
                _context.Add(territories);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id", territories.RegionId);
            return View(territories);
        }

        // GET: Territories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territories = await _context.Territories.FindAsync(id);
            if (territories == null)
            {
                return NotFound();
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id", territories.RegionId);
            return View(territories);
        }

        // POST: Territories/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,TerritoryId,TerritoryDescription,RegionId")] Territories territories)
        {
            if (id != territories.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(territories);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerritoriesExists(territories.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["RegionId"] = new SelectList(_context.Region, "Id", "Id", territories.RegionId);
            return View(territories);
        }

        // GET: Territories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var territories = await _context.Territories
                .Include(t => t.Region)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (territories == null)
            {
                return NotFound();
            }

            return View(territories);
        }

        // POST: Territories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var territories = await _context.Territories.FindAsync(id);
            _context.Territories.Remove(territories);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerritoriesExists(int id)
        {
            return _context.Territories.Any(e => e.Id == id);
        }
    }
}
