using Timashev_PI_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Timashev_PI_Lab.Logic
{
    public class TechCardLogic
    {
        private Database context;

        public TechCardLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(TechCard techCard)
        {
            TechCard tempTechCard = techCard.Id.HasValue ? null : new TechCard();

            if (techCard.Id.HasValue)
            {
                tempTechCard = context.TechCards.FirstOrDefault(rec => rec.Id == techCard.Id);
            }

            if (techCard.Id.HasValue)
            {
                if (tempTechCard == null)
                {
                    throw new Exception("Элемент не найден");
                }

                if (techCard.DeleteMark != null)
                {
                    tempTechCard.DeleteMark = techCard.DeleteMark;
                }
                else
                {
                    tempTechCard.Id = techCard.Id;
                    tempTechCard.Recipe = techCard.Recipe;
                }
            }
            else
            {
                context.TechCards.Add(techCard);
            }

            context.SaveChanges();
        }

        public void Delete(TechCard techCard)
        {
            TechCard element = context.TechCards.FirstOrDefault(rec => rec.Id == techCard.Id.Value);

            if (element != null)
            {
                context.TechCards.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<TechCard> Read(TechCard techCard)
        {
            List<TechCard> result = new List<TechCard>();

            if (techCard != null)
            {
                if (techCard.DeleteMark != null)
                {
                    result.AddRange(context.TechCards
                   .Include(rec => rec.Recipe)
                   .ThenInclude(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                   .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                   .Where(rec => rec.DeleteMark == techCard.DeleteMark)
                   .Select(rec => rec));
                    return result;
                }
                result.AddRange(context.TechCards
                    .Include(rec => rec.Recipe)
                    .ThenInclude(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Where(rec => (rec.Id == techCard.Id))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.TechCards);
            }

            return result;
        }
    }
}
