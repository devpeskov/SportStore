using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Cart
    {
        private List<CartLine> LineCollection = new List<CartLine>();

        public IEnumerable<CartLine> Lines { get { return LineCollection; } }

        public void AddItem(Product product, int quantity)
        {
            CartLine line = LineCollection
                .Where(b => b.Product.ProductId == product.ProductId)
                .FirstOrDefault();

            if (line == null)
            {
                LineCollection.Add(new CartLine { Product = product, Quantity = quantity });
            }
            else
            {
                line.Quantity += quantity;
            }
        }
        public void RemoveLine(Product product)
        {
            LineCollection.RemoveAll(m => m.Product.ProductId == product.ProductId);
        }
        public decimal ComputeTotalValue()
        {
            return LineCollection.Sum(m => m.Product.Price * m.Quantity);
        }

        public void Clear()
        {
            LineCollection.Clear();
        }

    }
    public class CartLine
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }
}
