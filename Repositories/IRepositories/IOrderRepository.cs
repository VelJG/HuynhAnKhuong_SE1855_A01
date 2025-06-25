using BusinessObjects;
using System;
using System.Collections.Generic;

namespace Repositories.IRepositories
{
    public interface IOrderRepository : IGenericRepository<Order>
    {
       IEnumerable<Order> SearchOrders(string search);
    }
}