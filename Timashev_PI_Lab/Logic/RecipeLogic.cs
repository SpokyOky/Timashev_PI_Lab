using Timashev_PI_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Timashev_PI_Lab.Logic
{
    public class RecipeLogic
    {
        private Database context;

        public RecipeLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(Recipe recipe)
        {
            Recipe tempRecipe = recipe.Id.HasValue ? null : new Recipe();

            if (recipe.Id.HasValue)
            {
                tempRecipe = context.Recipes.FirstOrDefault(rec => rec.Id == recipe.Id);
            }

            if (recipe.Id.HasValue)
            {
                if (tempRecipe == null)
                {
                    throw new Exception("Элемент не найден");
                }

                if (recipe.DeleteMark != null)
                {
                    tempRecipe.DeleteMark = recipe.DeleteMark;
                }
                else
                {
                    if (recipe.HowToCook != null)
                    {
                        tempRecipe.HowToCook = recipe.HowToCook;
                    }
                    if (recipe.Quality != null)
                    {
                        tempRecipe.Quality = recipe.Quality;
                    }

                    tempRecipe.Id = recipe.Id;
                    tempRecipe.Name = recipe.Name;
                    tempRecipe.ProductRecipes = recipe.ProductRecipes;
                }
            }
            else
            {
                context.Recipes.Add(recipe);
            }

            context.SaveChanges();
        }

        public void Delete(Recipe recipe)
        {
            Recipe element = context.Recipes.FirstOrDefault(rec => rec.Id == recipe.Id.Value);

            if (element != null)
            {
                context.Recipes.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<Recipe> Read(Recipe recipe)
        {
            List<Recipe> result = new List<Recipe>();

            if (recipe != null)
            {
                if (recipe.DeleteMark != null)
                {
                    result.AddRange(context.Recipes
                       .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                       .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                       .Include(rec => rec.TechCards)
                       .Where(rec => rec.DeleteMark == recipe.DeleteMark)
                       .Select(rec => rec));
                    return result;
                }
                result.AddRange(context.Recipes
                    .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Include(rec => rec.TechCards)
                    .Where(rec => (rec.Id == recipe.Id) || (rec.Name == recipe.Name))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Recipes);
            }

            return result;
        }
    }
}
