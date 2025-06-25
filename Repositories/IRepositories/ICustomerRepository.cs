
using BusinessObjects;

namespace Repositories.IRepositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        IEnumerable<Customer> SearchCustomers(string search);
        Customer Login (string phoneNumber);
    }
}