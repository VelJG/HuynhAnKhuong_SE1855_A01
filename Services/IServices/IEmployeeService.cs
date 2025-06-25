using BusinessObjects;
using System.Collections.Generic;

namespace Services.IServices
{
    public interface IEmployeeService : IGenericService<Employee>
    {
        Employee Login(string username, string password);
    }
}