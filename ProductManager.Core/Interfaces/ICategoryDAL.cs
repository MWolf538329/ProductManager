using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface ICategoryDAL
    {
        Category GetCategory(int id);
        List<Category> GetCategories();
        bool CreateCategory(string name);
        bool UpdateCategory(int id, string name);
        bool DeleteCategory(int id);
    }
}
