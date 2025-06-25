using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDAO _orderDAO;
        public OrderRepository(OrderDAO orderDAO)
        {
            _orderDAO = orderDAO;
        }
        public bool Add(Order entity)
        {
            return _orderDAO.Add(entity);
        }

        public bool Delete(int id)
        {
            return _orderDAO.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderDAO.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderDAO.GetById(id);
        }

        public IEnumerable<Order> SearchOrders(string search)
        {
            return _orderDAO.Search(search);
        }

        public bool Update(Order entity)
        {
            return _orderDAO.Update(entity);
        }
    }
}