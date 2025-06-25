using BusinessObjects;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using System.Collections.Generic;
using System.Linq;

namespace Services.Services
{
    public class OrderDetailService : IOrderDetailService
    {
        private readonly IOrderDetailRepository _orderDetailRepository;

       public OrderDetailService(IOrderDetailRepository orderDetailRepository)
        {
            _orderDetailRepository = orderDetailRepository;
        }
        public bool Add(OrderDetail entity)
        {
            return _orderDetailRepository.Add(entity);
        }

        public bool Delete(int id)
        {
            return _orderDetailRepository.Delete(id);
        }

        public bool DeleteOrderDetail(int orderId, int productId)
        {
            return _orderDetailRepository.DeleteOrderDetail(orderId, productId);
        }

        public IEnumerable<OrderDetail> GetAll()
        {
            return _orderDetailRepository.GetAll();
        }

        public OrderDetail GetById(int id)
        {
            return _orderDetailRepository.GetById(id);
        }

        public IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId)
        {
            return _orderDetailRepository.GetOrderDetailsByOrderId(orderId);
        }

        public bool Update(OrderDetail entity)
        {
            return _orderDetailRepository.Update(entity);
        }
    }
}