using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

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
            Category category = new();

            category = _DAL.GetCategory(id);

            if (category.ID == 0) throw new Exception("Category ID can not be 0!");
            if (string.IsNullOrEmpty(category.Name)) throw new Exception("Category Name can not be empty!");

            return category;
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

        public string CreateCategory(string name)
        {
            return _DAL.CreateCategory(name);
        }

        public string UpdateCategory(int id, string name)
        {
            return _DAL.UpdateCategory(id, name);
        }

        public string DeleteCategory(int id)
        {
            return _DAL.DeleteCategory(id);
        }
    }
}
