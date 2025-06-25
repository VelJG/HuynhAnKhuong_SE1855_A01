using BusinessObjects;
using System.Collections.Generic;

namespace Repositories.IRepositories
{
    public interface IEmployeeRepository : IGenericRepository<Employee>
    {
        Employee Login(string username, string password);
    }
}