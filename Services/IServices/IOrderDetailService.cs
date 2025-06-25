using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface IOrderDetailService : IGenericService<OrderDetail>
    {
        bool DeleteOrderDetail(int orderId, int productId);
        IEnumerable<OrderDetail> GetOrderDetailsByOrderId(int orderId);
    }
}