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
    public class ShippersController : Controller
    {
        private readonly Context _context;
		private readonly IConfiguration _configuration;

		public ShippersController(IConfiguration configuration)
		{
            _configuration = configuration;
		}

        // GET: Shippers
        public async Task<IActionResult> Index()
        {
			var shippers = new ServiceShippers(_configuration);
			var _results = await shippers.GetShippers();

			return View(_results);
        }

        // GET: Shippers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

			var shippers = new ServiceShippers(_configuration);
			var _result = await shippers.GetShipper(id.Value);

			if (_result == null)
            {
                return NotFound();
            }

            return View(_result);
        }

        // GET: Shippers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Shippers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,ShipperId,CompanyName,Phone")] Shippers shippers)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shippers);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(shippers);
        }

        // GET: Shippers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippers = await _context.Shippers.FindAsync(id);
            if (shippers == null)
            {
                return NotFound();
            }
            return View(shippers);
        }

        // POST: Shippers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ShipperId,CompanyName,Phone")] Shippers shippers)
        {
            if (id != shippers.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shippers);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShippersExists(shippers.Id))
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
            return View(shippers);
        }

        // GET: Shippers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shippers = await _context.Shippers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (shippers == null)
            {
                return NotFound();
            }

            return View(shippers);
        }

        // POST: Shippers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var shippers = await _context.Shippers.FindAsync(id);
            _context.Shippers.Remove(shippers);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ShippersExists(int id)
        {
            return _context.Shippers.Any(e => e.Id == id);
        }
    }
}
