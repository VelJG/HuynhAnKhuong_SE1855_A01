using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerDAO _customerDAO;
        public CustomerRepository(CustomerDAO customer)
        {
            _customerDAO = customer;
        }
        public bool Add(Customer entity)
        {
            return _customerDAO.Add(entity);
        }

        public Customer Login(string phoneNumber)
        {
            return _customerDAO.Login(phoneNumber);
        }

        public bool Delete(int id)
        {
            return _customerDAO.Delete(id);
        }

        public IEnumerable<Customer> GetAll()
        {
            return _customerDAO.GetAll();
        }

        public Customer GetById(int id)
        {
            return _customerDAO.GetById(id);
        }

        public IEnumerable<Customer> SearchCustomers(string search)
        {
            return _customerDAO.Search(search);
        }

        public bool Update(Customer entity)
        {
            return _customerDAO.Update(entity);
        }
    }
}