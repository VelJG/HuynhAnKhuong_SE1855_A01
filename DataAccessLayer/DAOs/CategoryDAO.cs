using BusinessObjects;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccessLayer.DAOs
{
    public class CategoryDAO : IGenericDAO<Category>
    {
        // Using the same static collection to maintain object instances
        private static List<Category> _categories = new List<Category>
        {
            new Category(1, "Beverages", "Soft drinks, coffees, teas, beers, and ales"),
            new Category(2, "Condiments", "Sweet and savory sauces, relishes, spreads, and seasonings"),
            new Category(3, "Confections", "Desserts, candies, and sweet breads"),
            new Category(4, "Dairy Products", "Cheeses"),
            new Category(5, "Grains/Cereals", "Breads, crackers, pasta, and cereal")
        };

        public IEnumerable<Category> GetAll()
        {
            return _categories;
        }

        public Category GetById(int id)
        {
            return _categories.FirstOrDefault(c => c.CategoryId == id);
        }

        public bool Add(Category entity)
        {

            if (_categories.Any(c => c.CategoryId == entity.CategoryId))
            {
                return false;
            }

            _categories.Add(entity);
            return true;

        }

        public bool Update(Category entity)
        {

            var category = _categories.FirstOrDefault(c => c.CategoryId == entity.CategoryId);
            if (category != null)
            {
                category.CategoryName = entity.CategoryName;
                category.Description = entity.Description;
                return true;
            }
            return false;
        }

        public bool Delete(int id)
        {

            var category = _categories.FirstOrDefault(c => c.CategoryId == id);
            if (category != null)
            {
                _categories.Remove(category);
                return true;

            }
            return false;
        }
        public IEnumerable<Category> Search(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return _categories;

            search = search.ToLower();
            return _categories.Where(c =>
                c.CategoryName.ToLower().Contains(search) ||
                (c.Description != null && c.Description.ToLower().Contains(search)));
        }
    }
}