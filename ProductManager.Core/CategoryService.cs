using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using System.Reflection.Metadata.Ecma335;
namespace ProductManager.Core
{
    public class CategoryService
    {
        ICategoryDAL _DAL;

        public CategoryService(ICategoryDAL dal)
        {
            _DAL = dal;
        }

        public Category GetCategory(int id)
        {
            return _DAL.GetCategory(id);
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

        public bool CreateCategory(string name)
        {
            return _DAL.CreateCategory(name);
        }

        public bool UpdateCategory(int id, string name)
        {
            return _DAL.UpdateCategory(id, name);
        }

        public bool DeleteCategory(int id)
        {
            return _DAL.DeleteCategory(id);
        }
    }
}
