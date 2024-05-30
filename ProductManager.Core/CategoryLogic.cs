using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
namespace ProductManager.Core
{
    public class CategoryLogic
    {
        ICategoryDAL _DAL;

        public CategoryLogic(ICategoryDAL dal)
        {
            _DAL = dal;
        }

        public IEnumerable<Category> GetCategories()
        {
            List<Category> categories = new();

            foreach (Category category in _DAL.GetCategories())
            {
                Category newCategory = new Category()
                {
                    ID = category.ID,
                    Name = category.Name
                };

                categories.Add(newCategory);
            }

            return categories;
        }
    }
}
