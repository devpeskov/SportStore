using Domain.Abstract;
using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.EF
{
    public class EFProductRepository : IProductRepository
    {
        ApplicationContext context = new ApplicationContext();
        public IEnumerable<Product> Products 
        {
            get {return context.Products; }
        }

        public void SaveProduct(Product product)
        {
            if (product.ProductId == 0)
            {
                context.Products.Add(product);
            }
            else
            {
                Product dbEntry = context.Products.Find(product.ProductId);
                if (dbEntry != null)
                {
                    dbEntry.Name = product.Name;
                    dbEntry.Brand = product.Brand;
                    dbEntry.Description = product.Description;
                    dbEntry.Type = product.Type;
                    dbEntry.Price = product.Price;
                    dbEntry.Image = product.Image;
                }
            }
            context.SaveChanges();
        }
        public void DeleteProduct(int id)
        {
            Product dbEntry = context.Products.Find(id);
            context.Products.Remove(dbEntry);
            context.SaveChanges();
        }

    }
}
