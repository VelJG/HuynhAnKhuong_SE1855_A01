using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
      private readonly OrderDetailDAO _orderDetailDAO;
        public OrderDetailRepository(OrderDetailDAO orderDetailDAO)
        {
            _orderDetailDAO = orderDetailDAO;
        }
        public bool Add(OrderDetail entity)
        {
            return _orderDetailDAO.Add(entity);
        }

        public bool Delete(int id)
        {
            return _orderDetailDAO.Delete(id);
        }

        public bool DeleteOrderDetail(int orderId, int productId)
        {
            return _orderDetailDAO.Delete(orderId, productId);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetailDAO.GetAll();
        }

        public OrderDetail GetById(int id)
        {
            return _orderDetailDAO.GetById(id);
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailDAO.GetByOrderId(orderId);
        }

        public bool Update(OrderDetail entity)
        {
            return _orderDetailDAO.Update(entity);
        }
    }
}   