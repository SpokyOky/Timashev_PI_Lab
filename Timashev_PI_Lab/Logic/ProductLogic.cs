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

        public void CreateOrUpdate(Product product)
        {
            Product tempProduct = product.Id.HasValue ? null : new Product();

            if (product.Id.HasValue)
            {
                tempProduct = context.Products.FirstOrDefault(rec => rec.Id == product.Id);
            }

            if (product.Id.HasValue)
            {
                if (tempProduct == null)
                {
                    throw new Exception("Элемент не найден");
                }
                if (product.DeleteMark != null)
                {
                    tempProduct.DeleteMark = product.DeleteMark;
                } else
                {
                    tempProduct.Id = product.Id;
                    tempProduct.Name = product.Name;
                    tempProduct.ProductChemElements = product.ProductChemElements;
                    tempProduct.ProductRecipes = product.ProductRecipes;
                }
            }
            else
            {
                context.Products.Add(product);
            }

            context.SaveChanges();
        }

        public void Delete(Product product)
        {
            Product element = context.Products.FirstOrDefault(rec => rec.Id == product.Id.Value);

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

        public List<Product> Read(Product product)
        {
            List<Product> result = new List<Product>();
            if (product != null)
            {
                if (product.DeleteMark != null)
                {
                    result.AddRange(context.Products
                    .Include(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Recipe)
                    .ThenInclude(rec => rec.TechCards)
                    .Where(rec => rec.DeleteMark == product.DeleteMark)
                    .Select(rec => rec));
                    return result;
                }
                result.AddRange(context.Products
                    .Include(rec => rec.ProductChemElements).ThenInclude(rec => rec.ChemElement)
                    .Include(rec => rec.ProductRecipes).ThenInclude(rec => rec.Recipe)
                    .ThenInclude(rec => rec.TechCards)
                    .Where(rec => (rec.Id == product.Id) || (rec.Name == product.Name))
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
