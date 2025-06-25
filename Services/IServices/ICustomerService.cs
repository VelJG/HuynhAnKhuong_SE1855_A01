using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface ICustomerService : IGenericService<Customer>
    {
        IEnumerable<Customer> Search(string keyword);
        Customer Login(string phoneNumber);
    }
}