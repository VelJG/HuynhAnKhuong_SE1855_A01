using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ProductDAO _productDAO;
       
        public ProductRepository(ProductDAO productDAO)
        {
            _productDAO = productDAO;
        }
        public bool Add(Product entity)
        {
            return _productDAO.Add(entity);
        }

        public bool Delete(int id)
        {
            return _productDAO.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productDAO.GetAll();
        }

        public Product GetById(int id)
        {
            return _productDAO.GetById(id);
        }

        public IEnumerable<Product> SearchProducts(string search)
        {
            return _productDAO.Search(search);
        }

        public bool Update(Product entity)
        {
            return _productDAO.Update(entity);
        }
    }
}