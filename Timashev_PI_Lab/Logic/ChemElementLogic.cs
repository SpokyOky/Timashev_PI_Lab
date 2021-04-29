using Timashev_PI_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Timashev_PI_Lab.Logic
{
    public class ChemElementLogic
    {
        private Database context;

        public ChemElementLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(ChemElement user)
        {
            ChemElement tempChemElement = user.Id.HasValue ? null : new ChemElement();

            if (user.Id.HasValue)
            {
                tempChemElement = context.ChemElements.FirstOrDefault(rec => rec.Id == user.Id);
            }

            if (user.Id.HasValue)
            {
                if (tempChemElement == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempChemElement.Id = user.Id;
                tempChemElement.Name = user.Name;
                tempChemElement.ProductChemElements = user.ProductChemElements;
            }
            else
            {
                context.ChemElements.Add(user);
            }

            context.SaveChanges();
        }

        public void Delete(ChemElement user)
        {
            ChemElement element = context.ChemElements.FirstOrDefault(rec => rec.Id == user.Id.Value);

            if (element != null)
            {
                context.ChemElements.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<ChemElement> Read(ChemElement user)
        {
            List<ChemElement> result = new List<ChemElement>();

            if (user != null)
            {
                result.AddRange(context.ChemElements
                    .Include(rec => rec.ProductChemElements).ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductRecipes).ThenInclude(rec => rec.Recipe)
                    .ThenInclude(rec => rec.RecipeTechCards).ThenInclude(rec => rec.TechCard)
                    .Where(rec => (rec.Id == user.Id) || (rec.Name == user.Name))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.ChemElements);
            }

            return result;
        }
    }
}
