using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAOs
{
    public class ProductDAO : IGenericDAO<Product>
    {
        private static List<Product> _products = new List<Product>
        {
            new Product(1, "Chai", 1, "10 boxes x 20 bags", 18.00m, 39, false),
            new Product(2, "Chang", 1, "24 - 12 oz bottles", 19.00M, 17, false),
            new Product(3, "Aniseed Syrup", 2, "12 - 550 ml bottles", 10.00M, 13, false),
            new Product(4, "Chef Anton's Cajun Seasoning", 2, "48 - 6 oz jars", 22.00M, 53, false),
            new Product(5, "Chef Anton's Gumbo Mix", 2, "36 boxes", 21.35M, 0, true),
        };

        public IEnumerable<Product> GetAll()
        {
            return _products;
        }

        public Product GetById(int id)
        {
            return _products.FirstOrDefault(p => p.ProductId == id);
        }

        public bool Add(Product entity)
        {
            try
            {
                if (_products.Any(p => p.ProductId == entity.ProductId))
                {
                    return false;
                }
                int newId = _products.Max(p => p.ProductId) + 1;
                entity.ProductId = newId;
                _products.Add(entity);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error adding product: {ex.Message}");
                return false;
            }
        }
        public bool Update(Product entity)
        {

            var product = _products.FirstOrDefault(p => p.ProductId == entity.ProductId);
            if (product != null)
            {
                product.ProductName = entity.ProductName;
                product.CategoryId = entity.CategoryId;
                product.QuantityPerUnit = entity.QuantityPerUnit;
                product.UnitPrice = entity.UnitPrice;
                product.UnitsInStock = entity.UnitsInStock;
                product.Discountinued = entity.Discountinued;
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {
            var product = _products.FirstOrDefault(p => p.ProductId == id);
            if (product != null)
            {
                _products.Remove(product);
                return true;
            }
            return false;
        }
        public IEnumerable<Product> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _products;

            search = search.ToLower();
            return _products.Where(p =>
                p.ProductName.ToLower().Contains(search));
        }
    }
}