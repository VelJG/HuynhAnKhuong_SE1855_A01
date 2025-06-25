using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAOs
{
    public class EmployeeDAO : IGenericDAO<Employee>
    {
        private static List<Employee> _employees = new List<Employee>
        {
            new Employee(1, "John Smith", "john", "1", "Manager", new DateTime(1968, 8, 30), new DateTime(2010, 5, 1), "Ho Chi Minh City"),
            new Employee(2, "Dave ManGuy", "dave", "1", "Sales", new DateTime(1952, 2, 19), new DateTime(2011, 8, 14), "Ha Noi"),
            new Employee(3, "Davie Jones", "davie", "1", "Sales", new DateTime(1963, 8, 30), new DateTime(2012, 4, 1), "Da Nang"),
        };

        public IEnumerable<Employee> GetAll()
        {
            return _employees;
        }

        public Employee GetById(int id)
        {
            return _employees.FirstOrDefault(e => e.EmployeeId == id);
        }

        public bool Add(Employee entity)
        {
            if (_employees.Any(e => e.EmployeeId == entity.EmployeeId))
            {
              return false;
            }
            _employees.Add(entity);
            return true;
        }

        public bool Update(Employee entity)
        {
            var employee = _employees.FirstOrDefault(e => e.EmployeeId == entity.EmployeeId);
            if (employee != null)
            {
                employee.Name = entity.Name;
                employee.UserName = entity.UserName;
                employee.Password = entity.Password;
                employee.JobTitle = entity.JobTitle;
                employee.BirthDate = entity.BirthDate;
                employee.HireDate = entity.HireDate;
                employee.Address = entity.Address;
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var employee = _employees.FirstOrDefault(e => e.EmployeeId == id);
            if (employee != null)
            {
                _employees.Remove(employee);
                return true;
            }
            return false;
        }

        public Employee Login(string username, string password)
        {
            return _employees.FirstOrDefault(e => 
                e.UserName.Trim().Equals(username, StringComparison.OrdinalIgnoreCase) && 
                e.Password.Trim().Equals(password));
        }
    }
}