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

        public void CreateOrUpdate(Recipe user)
        {
            Recipe tempRecipe = user.Id.HasValue ? null : new Recipe();

            if (user.Id.HasValue)
            {
                tempRecipe = context.Recipes.FirstOrDefault(rec => rec.Id == user.Id);
            }

            if (user.Id.HasValue)
            {
                if (tempRecipe == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempRecipe.Id = user.Id;
                tempRecipe.Name = user.Name;
                tempRecipe.ProductRecipes = user.ProductRecipes;
            }
            else
            {
                context.Recipes.Add(user);
            }

            context.SaveChanges();
        }

        public void Delete(Recipe user)
        {
            Recipe element = context.Recipes.FirstOrDefault(rec => rec.Id == user.Id.Value);

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

        public List<Recipe> Read(Recipe user)
        {
            List<Recipe> result = new List<Recipe>();

            if (user != null)
            {
                result.AddRange(context.Recipes
                    .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Include(rec => rec.RecipeTechCards).ThenInclude(rec => rec.TechCard)
                    .Where(rec => (rec.Id == user.Id) || (rec.Name == user.Name))
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
