using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAOs
{
    public class CustomerDAO : IGenericDAO<Customer>
    {
        private static List<Customer> _customers = new List<Customer>
        {
            new Customer(1, "Hololive", "Shiori Novella", "Archiver", "Tokyo Mita Garden Tower", "090952521"),
            new Customer(2, "Hololive", "Koseki Bijou", "Jewel Of Emotions", "Tokyo Mita Garden Tower", "090952522"),
            new Customer(3, "Hololive", "Nerissa Ravencroft", "Demon Of Sound", "Tokyo Mita Garden Tower", "090952523"),
            new Customer(4, "Hololive", "Fuwawa Abyssgard", "Fluffy One", "Tokyo Mita Garden Tower", "090952524"),
            new Customer(5, "Hololive", "Mococo Abyssgard", "Fuzzy One", "Tokyo Mita Garden Tower", "090952525")
        };

        public IEnumerable<Customer> GetAll()
        {
            return _customers;
        }

        public Customer GetById(int id)
        {
            return _customers.FirstOrDefault(c => c.CustomerId == id);
        }

        public bool Add(Customer entity)
        {

            if (_customers.Any(c => c.CustomerId == entity.CustomerId))
            {
                return false;
            }
            int newId = _customers.Max(c => c.CustomerId) + 1;
            entity.CustomerId = newId;
            _customers.Add(entity);
            return true;
        }

        public bool Update(Customer entity)
        {

            var customer = _customers.FirstOrDefault(c => c.CustomerId == entity.CustomerId);
            if (customer != null)
            {
                customer.CompanyName = entity.CompanyName;
                customer.ContactName = entity.ContactName;
                customer.ContactTitle = entity.ContactTitle;
                customer.Address = entity.Address;
                customer.Phone = entity.Phone;
                return true;
            }
            return false;

        }

        public bool Delete(int id)
        {

            var customer = _customers.FirstOrDefault(c => c.CustomerId == id);
            if (customer != null)
            {
                _customers.Remove(customer);
                return true;
            }
            return false;
        }

        public IEnumerable<Customer> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _customers;

            search = search.ToLower();
            return _customers.Where(c =>
                c.CompanyName.ToLower().Contains(search) ||
                c.ContactName.ToLower().Contains(search) ||
                c.ContactTitle.ToLower().Contains(search) ||
                c.Address.ToLower().Contains(search) ||
                c.Phone.Contains(search));
        }

        public Customer Login(string phoneNumber)
        {
            return _customers.FirstOrDefault(c =>
                c.Phone.Trim().Equals(phoneNumber, StringComparison.OrdinalIgnoreCase));
        }
    }
}