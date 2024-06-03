using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface ICategoryDAL
    {
        List<Category> GetCategories();
        Category GetCategory(int id);
        string CreateCategory(string name);
        string UpdateCategory(int id, string name);
        string DeleteCategory(int id);
    }
}
