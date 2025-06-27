using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAOs
{
    public class OrderDetailDAO : IGenericDAO<OrderDetail>
    {
        private static List<OrderDetail> _orderDetails = new List<OrderDetail>
        {
            new OrderDetail(1, 1, 18.00M, 2, 0),
            new OrderDetail(2, 3, 10.00M, 1, 0),
            new OrderDetail(3, 2, 19.00M, 3, 0.05),
            new OrderDetail(4, 4, 22.00M, 2, 0),
            new OrderDetail(5, 5, 21.35M, 4, 0.1)
        };

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetails;
        }

        public OrderDetail GetById(int id)
        {
            throw new NotImplementedException("OrderDetail does not have a single ID");
        }

        public bool Add(OrderDetail entity)
        {
            try
            {
                if (_orderDetails.Any(od => od.OrderId == entity.OrderId && od.ProductId == entity.ProductId))
                {
                    return false;
                }

                _orderDetails.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order detail: {ex.Message}");
                return false;
            }
        }
        public bool Update(OrderDetail entity)
        {
            var orderDetail = _orderDetails.FirstOrDefault(od =>
                od.OrderId == entity.OrderId && od.ProductId == entity.ProductId);

            if (orderDetail != null)
            {
                orderDetail.UnitPrice = entity.UnitPrice;
                orderDetail.Quantity = entity.Quantity;
                orderDetail.Discount = entity.Discount;
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            return false; // Not used   
        }

        public bool Delete(int orderId, int productId)
        {
            var orderDetail = _orderDetails.FirstOrDefault(od =>
                od.OrderId == orderId && od.ProductId == productId);

            if (orderDetail != null)
            {
                _orderDetails.Remove(orderDetail);
                return true;
            }
            return false;
        }
        public IEnumerable<OrderDetail> GetByOrderId(int orderId)
        {
            return _orderDetails.Where(od => od.OrderId == orderId);
        }
    }
}