using BusinessObjects;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using System.Collections.Generic;

namespace Services.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;

       public CustomerService(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }
        public bool Add(Customer entity)
        {
            return _customerRepository.Add(entity);
        }

        public bool Delete(int id)
        {
            return _customerRepository.Delete(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerRepository.GetAll();
        }

        public Customer GetById(int id)
        {
            return _customerRepository.GetById(id);
        }

        public Customer Login(string phoneNumber)
        {
            return _customerRepository.Login(phoneNumber);
        }

        public IEnumerable<Customer> Search(string search)
        {
            return _customerRepository.SearchCustomers(search);
        }

        public bool Update(Customer entity)
        {
            return _customerRepository.Update(entity);
        }
    }
}