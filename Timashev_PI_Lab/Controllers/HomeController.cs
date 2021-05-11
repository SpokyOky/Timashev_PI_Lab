using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Timashev_PI_Lab.Logic;
using Timashev_PI_Lab.Models;

namespace Timashev_PI_Lab.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private RecipeLogic _recipeLogic;
        private ProductLogic _productLogic;
        private TechCardLogic _techCardLogic;

        public HomeController(ILogger<HomeController> logger, TechCardLogic techCardLogic, RecipeLogic recipeLogic, ProductLogic productLogic)
        {
            _logger = logger;
            _recipeLogic = recipeLogic;
            _productLogic = productLogic;
            _techCardLogic = techCardLogic;
        }

        public IActionResult About()
        {
            return View();
        }

        public IActionResult Index()
        {
            if (Program.User != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        public IActionResult DeleteMark()
        {
            var techCards = _techCardLogic.Read(new TechCard { DeleteMark = true });
            foreach (var techCard in techCards)
            {
                _techCardLogic.Delete(techCard);
            }
            var recipes = _recipeLogic.Read(new Recipe { DeleteMark = true });
            foreach (var recipe in recipes)
            {
                if (recipe.TechCards != null)
                {
                    while (recipe.TechCards.Count() > 0)
                    {
                        _techCardLogic.Delete(recipe.TechCards[0]);
                    }
                }
                _recipeLogic.Delete(recipe);
            }
            var products = _productLogic.Read(new Product { DeleteMark = true });
            foreach (var product in products)
            {
                if (product.ProductRecipes != null)
                {
                    while (product.ProductRecipes.Count() > 0)
                    {
                        if (product.ProductRecipes[0].Recipe.TechCards != null)
                        {
                            while (product.ProductRecipes[0].Recipe.TechCards.Count() > 0)
                            {
                                _techCardLogic.Delete(product.ProductRecipes[0].Recipe.TechCards[0]);
                            }
                        }
                        _recipeLogic.Delete(product.ProductRecipes[0].Recipe);
                    }
                }
                _productLogic.Delete(product);
            }
            return View("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
