using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface ICategoryDAL
    {
        Category GetCategory(int id);
        List<Category> GetCategories();
        string CreateCategory(string name);
        string UpdateCategory(int id, string name);
        string DeleteCategory(int id);
    }
}
