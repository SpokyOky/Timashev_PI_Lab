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
    public class RecipesController : Controller
    {
        private readonly Database _context;
        private RecipeLogic _recipeLogic;
        private ProductLogic _productLogic;

        public RecipesController(Database context, RecipeLogic recipeLogic, ProductLogic productLogic)
        {
            _context = context;
            _recipeLogic = recipeLogic;
            _productLogic = productLogic;
        }

        // GET: Recipes
        public async Task<IActionResult> Index()
        {
            return View(await _context.Recipes.ToListAsync());
        }

        // GET: Recipes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = _recipeLogic.Read(new Recipe { Id = id }).First();
            if (recipe == null)
            {
                return NotFound();
            }

            ViewBag.ProductsList = GetProducts(recipe);
            return View(recipe);
        }

        // GET: Recipes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Recipes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Recipe recipe)
        {
            if (ModelState.IsValid)
            {
                _context.Add(recipe);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(recipe);
        }

        // GET: Recipes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = _recipeLogic.Read(new Recipe { Id = id }).First();
            if (recipe == null)
            {
                return NotFound();
            }
            ViewBag.ProductsList = GetProducts(recipe);
            return View(recipe);
        }

        // POST: Recipes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id,Name")] Recipe recipe)
        {
            if (id != recipe.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recipe);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecipeExists(recipe.Id))
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
            ViewBag.ProductsList = GetProducts(recipe);
            return View(recipe);
        }

        // GET: Recipes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var recipe = await _context.Recipes
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recipe == null)
            {
                return NotFound();
            }

            return View(recipe);
        }

        // POST: Recipes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var recipe = await _context.Recipes.FindAsync(id);
            _context.Recipes.Remove(recipe);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecipeExists(int? id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }

        public IActionResult AddProduct(int id)
        {
            var recipe = _recipeLogic.Read(new Recipe { Id = id }).First();
            var products = GetProducts(recipe, true);
            ViewBag.ProductsList = products;
            return View("AddProduct", recipe);
        }

        [HttpPost]
        public IActionResult AddProduct(Recipe model)
        {
            int productsId;

            if (Int32.TryParse(Request.Form["productlist"].ToString(), out productsId))
            {
                var form = Request.Form;
                if (form.ContainsKey("Gram") && form["Gram"][0].Length > 0)
                {
                    try
                    {
                        var gram = Convert.ToInt32(form["Gram"]);
                        if (gram > 0)
                        {
                            model.ProductRecipes = new List<ProductRecipe>();
                            model.ProductRecipes.Add(new ProductRecipe
                            {
                                Product = _productLogic.Read(
                                new Product { Id = productsId }).First(),
                                Recipe = model,
                                Gram = gram
                            });
                        }
                    }
                    catch (Exception) { }
                }
            }

            if (ModelState.IsValid)
            {
                _recipeLogic.CreateOrUpdate(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductsList = GetProducts(null);
                return View(nameof(AddProduct), model);
            }
        }

        private SelectList GetProducts(Recipe recipe, bool isAdd = false)
        {
            var _products = _productLogic.Read(null);
            if (recipe == null)
            {
                return new SelectList(_products, "Id", "Name");
            }

            var products = new List<Product>();
            if (recipe.ProductRecipes != null)
            {
                foreach (var pce in recipe.ProductRecipes)
                {
                    pce.Product.Gram = pce.Gram;
                    products.Add(pce.Product);
                }
            }

            if (isAdd)
            {
                foreach (var ce in products)
                {
                    _products.Remove(ce);
                }
                return new SelectList(_products, "Id", "Name");
            }
            var productGram = products.Select(ce => new
            {
                Id = ce.Id,
                Name = ce.Name + " - " + ce.Gram
            });
            return new SelectList(productGram, "Id", "Name");
        }

        public IActionResult RemoveProduct(int id)
        {
            var recipe = _recipeLogic.Read(new Recipe { Id = id }).First();
            ViewBag.ProductsList = GetProducts(recipe);
            return View("RemoveProduct", recipe);
        }

        [HttpPost]
        public IActionResult RemoveProduct(Recipe model)
        {
            int productsId;

            if (Int32.TryParse(Request.Form["productlist"].ToString(), out productsId))
            {
                model.ProductRecipes = new List<ProductRecipe>();
                model.ProductRecipes.Remove(new ProductRecipe
                {
                    Product = _productLogic.Read(
                    new Product { Id = productsId }).First(),
                    Recipe = model
                });
            }

            if (ModelState.IsValid)
            {
                _recipeLogic.CreateOrUpdate(model);
                return RedirectToAction("Index");
            }
            else
            {
                ViewBag.ProductsList = GetProducts(model);
                return View("RemoveProduct", model);
            }
        }
    }
}
