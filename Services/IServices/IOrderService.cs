using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface IOrderService : IGenericService<Order>
    {
        IEnumerable<Order> Search(string keyword);
    }
}