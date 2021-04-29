using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timashev_PI_Lab;
using Timashev_PI_Lab.Models;

namespace Timashev_PI_Lab.Controllers
{
    public class ChemElementsController : Controller
    {
        private readonly Database _context;

        public ChemElementsController(Database context)
        {
            _context = context;
        }

        // GET: ChemElements
        public async Task<IActionResult> Index()
        {
            return View(await _context.ChemElements.ToListAsync());
        }

        // GET: ChemElements/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemElement = await _context.ChemElements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chemElement == null)
            {
                return NotFound();
            }

            return View(chemElement);
        }

        // GET: ChemElements/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ChemElements/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] ChemElement chemElement)
        {
            if (ModelState.IsValid)
            {
                _context.Add(chemElement);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(chemElement);
        }

        // GET: ChemElements/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemElement = await _context.ChemElements.FindAsync(id);
            if (chemElement == null)
            {
                return NotFound();
            }
            return View(chemElement);
        }

        // POST: ChemElements/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] ChemElement chemElement)
        {
            if (id != chemElement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(chemElement);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ChemElementExists(chemElement.Id))
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
            return View(chemElement);
        }

        // GET: ChemElements/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var chemElement = await _context.ChemElements
                .FirstOrDefaultAsync(m => m.Id == id);
            if (chemElement == null)
            {
                return NotFound();
            }

            return View(chemElement);
        }

        // POST: ChemElements/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var chemElement = await _context.ChemElements.FindAsync(id);
            _context.ChemElements.Remove(chemElement);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ChemElementExists(int? id)
        {
            return _context.ChemElements.Any(e => e.Id == id);
        }
    }
}
