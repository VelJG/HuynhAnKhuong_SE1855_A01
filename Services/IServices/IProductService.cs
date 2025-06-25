using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface IProductService : IGenericService<Product>
    {
        IEnumerable<Product> Search(string keyword);
    }
}