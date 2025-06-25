using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObjects
{
    public partial class OrderDetail
    {
        public OrderDetail() { }
        public OrderDetail(int id, int productId, decimal price, int quantity, double discount)
        {
            OrderId=id;
            ProductId=productId;
            UnitPrice=price;
            Quantity=quantity;
            Discount=discount;
        }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public double Discount { get; set; }
        public decimal LineTotal => UnitPrice * Quantity * (decimal)(1 - Discount);
        public virtual Order Order { get; set; }
        public virtual Product Product { get; set; }
    }
}
