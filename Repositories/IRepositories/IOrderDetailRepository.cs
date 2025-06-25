using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.IRepositories
{
    public interface IOrderDetailRepository : IGenericRepository<OrderDetail>
    {
        bool DeleteOrderDetail(int orderId, int productId);
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
    }
}