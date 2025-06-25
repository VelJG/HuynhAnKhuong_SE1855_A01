using BusinessObjects;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using System.Collections.Generic;

namespace Services.Services
{
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        public EmployeeService(IEmployeeRepository employeeRepository)
        {
            _employeeRepository = employeeRepository;
        }

        public bool Add(Employee entity)
        {
            return _employeeRepository.Add(entity);
        }

        public Employee Login(string username, string password)
        {
            return _employeeRepository.Login(username, password);
        }

        public bool Delete(int id)
        {
            return _employeeRepository.Delete(id);
        }

        public IEnumerable<Employee> GetAll()
        {
            return _employeeRepository.GetAll();
        }

        public Employee GetById(int id)
        {
            return _employeeRepository.GetById(id);
        }

        public bool Update(Employee entity)
        {
            return _employeeRepository.Update(entity);
        }
    }
}