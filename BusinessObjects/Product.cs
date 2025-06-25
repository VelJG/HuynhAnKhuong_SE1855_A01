using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public partial class Product
    {

        public Product() { }

        public Product(int productId, string productName, int categoryId, string quantityPerUnit, decimal unitPrice, int unitsInStock, bool discountinued)
        {
            ProductId = productId;
            ProductName = productName;
            CategoryId = categoryId;
            QuantityPerUnit = quantityPerUnit;
            UnitPrice = unitPrice;
            UnitsInStock = unitsInStock;
            Discountinued = discountinued;
        }

        public int ProductId {  get; set; }
        public string ProductName { get; set; }
        public int CategoryId { get; set; }
        public string QuantityPerUnit { get; set; }
        public decimal UnitPrice { get; set; }
        public int UnitsInStock { get; set; }
        public bool Discountinued { get; set; }

        public virtual Category Category { get; set; }
    }
}
