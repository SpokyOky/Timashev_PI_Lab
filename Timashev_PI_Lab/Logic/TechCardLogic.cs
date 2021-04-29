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

        public void CreateOrUpdate(TechCard user)
        {
            TechCard tempTechCard = user.Id.HasValue ? null : new TechCard();

            if (user.Id.HasValue)
            {
                tempTechCard = context.TechCards.FirstOrDefault(rec => rec.Id == user.Id);
            }

            if (user.Id.HasValue)
            {
                if (tempTechCard == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempTechCard.Id = user.Id;
                tempTechCard.Name = user.Name;
                tempTechCard.RecipeTechCards = user.RecipeTechCards;
            }
            else
            {
                context.TechCards.Add(user);
            }

            context.SaveChanges();
        }

        public void Delete(TechCard user)
        {
            TechCard element = context.TechCards.FirstOrDefault(rec => rec.Id == user.Id.Value);

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

        public List<TechCard> Read(TechCard user)
        {
            List<TechCard> result = new List<TechCard>();

            if (user != null)
            {
                result.AddRange(context.TechCards
                    .Include(rec => rec.RecipeTechCards).ThenInclude(rec => rec.Recipe)
                    .ThenInclude(rec => rec.ProductRecipes).ThenInclude(rec => rec.Product)
                    .ThenInclude(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Where(rec => (rec.Id == user.Id) || (rec.Name == user.Name))
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
