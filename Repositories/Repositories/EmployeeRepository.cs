using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {

        private readonly EmployeeDAO _employeeDAO;

        public EmployeeRepository(EmployeeDAO employeeDAO)
        {
            _employeeDAO = employeeDAO;
        }
        public EmployeeRepository()
        {
            _employeeDAO = new EmployeeDAO();
        }
        public bool Add(Employee entity)
        {
            return _employeeDAO.Add(entity);
        }

        public Employee Login(string username, string password)
        {

            return _employeeDAO.Login(username, password);
        }

        public bool Delete(int id)
        {
            return _employeeDAO.Delete(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeDAO.GetAll();
        }

        public Employee GetById(int id)
        {
            return _employeeDAO.GetById(id);
        }

        public bool Update(Employee entity)
        {
            return _employeeDAO.Update(entity);
        }
    }
}