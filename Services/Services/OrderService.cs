using BusinessObjects;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using System.Collections.Generic;

namespace Services.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }
        public bool Add(Order entity)
        {
            return _orderRepository.Add(entity);
        }

        public bool Delete(int id)
        {
            return _orderRepository.Delete(id);
        }

        public IEnumerable<Order> GetAll()
        {
            return _orderRepository.GetAll();
        }

        public Order GetById(int id)
        {
            return _orderRepository.GetById(id);
        }

        public IEnumerable<Order> Search(string search)
        {
            return _orderRepository.SearchOrders(search);
        }

        public bool Update(Order entity)
        {
            return _orderRepository.Update(entity);
        }
    }
}