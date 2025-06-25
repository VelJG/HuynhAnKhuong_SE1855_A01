
using BusinessObjects;

namespace Repositories.IRepositories
{
    public interface ICategoryRepository : IGenericRepository<Category>
    {
        IEnumerable<Category> SearchCategories(string search);
    }
}