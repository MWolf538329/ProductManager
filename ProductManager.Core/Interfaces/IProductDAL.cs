using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IProductDAL
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
        List<Category> GetCategories();
    }
}
