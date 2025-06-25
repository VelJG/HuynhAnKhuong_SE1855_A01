using BusinessObjects;
using Repositories.IRepositories;
using Repositories.Repositories;
using Services.IServices;
using System.Collections.Generic;

namespace Services.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public bool Add(Product entity)
        {
            return _productRepository.Add(entity);
        }

        public bool Delete(int id)
        {
            return _productRepository.Delete(id);
        }

        public IEnumerable<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product GetById(int id)
        {
            return _productRepository.GetById(id);
        }

        public IEnumerable<Product> Search(string search)
        {
            return _productRepository.SearchProducts(search);
        }

        public bool Update(Product entity)
        {
            return _productRepository.Update(entity);
        }
    }
}