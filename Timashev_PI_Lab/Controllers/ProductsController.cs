using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Timashev_PI_Lab;
using Timashev_PI_Lab.Models;
using Timashev_PI_Lab.Logic;

namespace Timashev_PI_Lab.Controllers
{
    public class ProductsController : Controller
    {
        private readonly Database _context;
        private ChemElementLogic _chemElementLogic;
        private ProductLogic _productLogic;

        public ProductsController(Database context, ProductLogic productLogic, ChemElementLogic chemElementLogic)
        {
            _context = context;
            _chemElementLogic = chemElementLogic;
            _productLogic = productLogic;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());
        }


        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productLogic.Read(new Product { Id = id }).First();
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ChemElementsList = GetChemElements(product);
            return View(product);
        }

        public IActionResult UnmarkDelete(int? id)
        {
            _productLogic.CreateOrUpdate(new Product { Id = id, DeleteMark = false });
            return RedirectToAction("Index", "Products");
        }

        public IActionResult MarkDelete(int? id)
        {
            _productLogic.CreateOrUpdate(new Product { Id = id, DeleteMark = true });
            return RedirectToAction("Index", "Products");
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Product product)
        {
            if (ModelState.IsValid)
            {
                if (string.IsNullOrWhiteSpace(product.Name))
                {
                    ModelState.AddModelError("Error", "Введите название");
                    return View(product);
                }
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = _productLogic.Read(new Product { Id = id }).First();
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.ChemElementsList = GetChemElements(product);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (string.IsNullOrWhiteSpace(product.Name))
                    {
                        ModelState.AddModelError("Error", "Введите название");
                        ViewBag.ChemElementsList = GetChemElements(product);
                        return View(product);
                    }
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
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
            ViewBag.ChemElementsList = GetChemElements(product);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int? id)
        {
            return _context.Products.Any(e => e.Id == id);
        }

        public IActionResult AddChemElement(int id)
        {
            var product = _productLogic.Read(new Product { Id = id }).First();
            var chemElements = GetChemElements(product, true);
            ViewBag.ChemElementsList = chemElements;
            return View("AddChemElement", product);
        }

        [HttpPost]
        public IActionResult AddChemElement(Product model)
        {
            int chemElementsId;

            if (Int32.TryParse(Request.Form["chemElementlist"].ToString(), out chemElementsId))
            {
                var form = Request.Form;
                if (form.ContainsKey("Gramm") && form["Gramm"][0].Length > 0)
                {
                    try
                    {
                        var gram = Convert.ToDecimal(form["Gramm"].ToString().Replace('.', ','));
                        if (gram > 0)
                        {
                            model.ProductChemElements = new List<ProductChemElement>();
                            model.ProductChemElements.Add(new ProductChemElement
                            {
                                ChemElement = _chemElementLogic.Read(
                                new ChemElement { Id = chemElementsId }).First(),
                                Product = model,
                                Gram = gram
                            });
                        }
                    }
                    catch (Exception) { }
                }
            }

            if (ModelState.IsValid)
            {
                _productLogic.CreateOrUpdate(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ChemElementsList = GetChemElements(null);
                return View(nameof(AddChemElement), model);
            }
        }

        private SelectList GetChemElements(Product product, bool isAdd = false)
        {
            var _chemElements = _chemElementLogic.Read(null);
            if (product == null)
            {
                return new SelectList(_chemElements, "Id", "Name");
            }

            var chemElements = new List<ChemElement>();
            if (product.ProductChemElements != null)
            {
                foreach (var pce in product.ProductChemElements)
                {
                    pce.ChemElement.Gram = pce.Gram;
                    chemElements.Add(pce.ChemElement);
                }
            }

            if (isAdd)
            {
                foreach (var ce in chemElements)
                {
                    _chemElements.Remove(ce);
                }
                return new SelectList(_chemElements, "Id", "Name");
            }
            var chemElementGram = chemElements.Select(ce => new
            {
                Id = ce.Id,
                Name = ce.Name + " - " + ce.Gram
            });
            return new SelectList(chemElementGram, "Id", "Name");
        }

        public IActionResult RemoveChemElement(int id)
        {
            var product = _productLogic.Read(new Product { Id = id }).First();
            ViewBag.ChemElementsList = GetChemElements(product);
            return View("RemoveChemElement", product);
        }

        [HttpPost]
        public IActionResult RemoveChemElement(Product model)
        {
            int chemElementsId;

            if (Int32.TryParse(Request.Form["chemElementlist"].ToString(), out chemElementsId))
            {
                model.ProductChemElements = new List<ProductChemElement>();
                model.ProductChemElements.Remove(new ProductChemElement
                {
                    ChemElement = _chemElementLogic.Read(
                    new ChemElement { Id = chemElementsId }).First(),
                    Product = model
                });
            }

            if (ModelState.IsValid)
            {
                _productLogic.CreateOrUpdate(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ChemElementsList = GetChemElements(model);
                return View("RemoveChemElement", model);
            }
        }
    }
}
