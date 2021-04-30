using Timashev_PI_Lab.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Timashev_PI_Lab.Logic
{
    public class ProductLogic
    {
        private Database context;

        public ProductLogic(Database _context)
        {
            context = _context;
        }

        public void CreateOrUpdate(Product user)
        {
            Product tempProduct = user.Id.HasValue ? null : new Product();

            if (user.Id.HasValue)
            {
                tempProduct = context.Products.FirstOrDefault(rec => rec.Id == user.Id);
            }

            if (user.Id.HasValue)
            {
                if (tempProduct == null)
                {
                    throw new Exception("Элемент не найден");
                }

                tempProduct.Id = user.Id;
                tempProduct.Name = user.Name;
                tempProduct.ProductChemElements = user.ProductChemElements;
                tempProduct.ProductRecipes = user.ProductRecipes;
            }
            else
            {
                context.Products.Add(user);
            }

            context.SaveChanges();
        }

        public void Delete(Product user)
        {
            Product element = context.Products.FirstOrDefault(rec => rec.Id == user.Id.Value);

            if (element != null)
            {
                context.Products.Remove(element);
                context.SaveChanges();
            }
            else
            {
                throw new Exception("Элемент не найден");
            }
        }

        public List<Product> Read(Product user)
        {
            List<Product> result = new List<Product>();

            if (user != null)
            {
                result.AddRange(context.Products
                    .Include(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Recipe)
                    .ThenInclude(rec => rec.TechCards)
                    .Where(rec => (rec.Id == user.Id) || (rec.Name == user.Name))
                    .Select(rec => rec));
            }
            else
            {
                result.AddRange(context.Products);
            }

            return result;
        }
    }
}
