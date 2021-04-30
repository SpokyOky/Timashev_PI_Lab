using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timashev_PI_Lab;
using Timashev_PI_Lab.Logic;
using Timashev_PI_Lab.Models;

namespace Timashev_PI_Lab.Controllers
{
    public class TechCardsController : Controller
    {
        private readonly Database _context;
        private RecipeLogic _recipeLogic;
        private ProductLogic _productLogic;
        private ChemElementLogic _chemElementLogic;
        private TechCardLogic _techCardLogic;

        public TechCardsController(Database context, TechCardLogic techCardLogic, RecipeLogic recipeLogic, ProductLogic productLogic, ChemElementLogic chemElementLogic)
        {
            _context = context;
            _recipeLogic = recipeLogic;
            _productLogic = productLogic;
            _techCardLogic = techCardLogic;
        }

        // GET: TechCards
        public async Task<IActionResult> Index()
        {
            return View(await _context.TechCards.ToListAsync());
        }

        // GET: TechCards/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techCard = _techCardLogic.Read(new TechCard { Id = id }).First();
            if (techCard == null)
            {
                return NotFound();
            }

            return View(techCard);
        }

        // GET: TechCards/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: TechCards/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] TechCard techCard)
        {
            if (ModelState.IsValid)
            {
                _context.Add(techCard);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(techCard);
        }

        // GET: TechCards/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var techCard = await _context.TechCards
                .FirstOrDefaultAsync(m => m.Id == id);
            if (techCard == null)
            {
                return NotFound();
            }

            return View(techCard);
        }

        // POST: TechCards/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var techCard = await _context.TechCards.FindAsync(id);
            _context.TechCards.Remove(techCard);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TechCardExists(int? id)
        {
            return _context.TechCards.Any(e => e.Id == id);
        }
    }
}
