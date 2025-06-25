using BusinessObjects;
using DataAccessLayer.DAOs;
using Repositories.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositories.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly CategoryDAO _categoryDAO;
        public CategoryRepository(CategoryDAO categoryDAO)
        {
            _categoryDAO = categoryDAO;
        }
        public bool Add(Category entity)
        {
            return _categoryDAO.Add(entity);
        }

        public bool Delete(int id)
        {
            return _categoryDAO.Delete(id);
        }

        public IEnumerable<Category> GetAll()
        {
            return _categoryDAO.GetAll();
        }

        public Category GetById(int id)
        {
            return _categoryDAO.GetById(id);
        }

        public IEnumerable<Category> SearchCategories(string search)
        {
            return _categoryDAO.Search(search);
        }

        public bool Update(Category entity)
        {
            return _categoryDAO.Update(entity);
        }
    }
}