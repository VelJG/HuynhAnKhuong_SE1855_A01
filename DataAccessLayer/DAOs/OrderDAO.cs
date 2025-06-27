using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace DataAccessLayer.DAOs
{
    public class OrderDAO : IGenericDAO<Order>
    {
        private static List<Order> _orders = new List<Order>
        {
            new Order(1, 1, 1, DateTime.Parse("2025-06-15")),
            new Order(2, 2, 3, DateTime.Parse("2025-06-10")),
            new Order(3, 3, 2, DateTime.Parse("2025-06-05")),
            new Order(4, 4, 1, DateTime.Parse("2025-06-20")),
            new Order(5, 5, 2, DateTime.Parse("2025-06-12"))
        };

        public IEnumerable<Order> GetAll()
        {
            return _orders;
        }

        public Order GetById(int id)
        {
            return _orders.FirstOrDefault(o => o.OrderId == id);
        }

        public bool Add(Order entity)
        {
            try
            {
                if (_orders.Any(o => o.OrderId == entity.OrderId))
                {
                    return false;

                }
                _orders.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding order: {ex.Message}");
                return false;
            }
        }

        public bool Update(Order entity)
        {
            try
            {
                var order = _orders.FirstOrDefault(o => o.OrderId == entity.OrderId);
                if (order != null)
                {
                    order.CustomerId = entity.CustomerId;
                    order.EmployeeId = entity.EmployeeId;
                    order.OrderDate = entity.OrderDate;
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                return false;
            }
        }

        public bool Delete(int id)
        {
            var order = _orders.FirstOrDefault(o => o.OrderId == id);
            if (order != null)
            {
                _orders.Remove(order);
                return true;
            }
            return false;
        }
        public IEnumerable<Order> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _orders;

            if (int.TryParse(search, out int numericValue))
            {
                return _orders.Where(o =>
                    o.CustomerId == numericValue ||
                    o.EmployeeId == numericValue);
            }

            return new List<Order>();
        }
    }
}