using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.IRepositories
{
    public interface IProductRepository : IGenericRepository<Product>
    {
       
        IEnumerable<Product> SearchProducts(string search);
    }
}