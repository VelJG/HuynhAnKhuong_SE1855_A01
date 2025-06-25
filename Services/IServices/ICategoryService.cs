using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface ICategoryService : IGenericService<Category>
    {
        IEnumerable<Category> Search(string keyword);
    }
}