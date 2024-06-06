using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IProductDAL
    {
        List<Product> GetProducts();
        Product GetProduct(int id);
        string CreateProduct(string name, string brand, string categoryName, decimal price, int contents, string unit);
        string UpdateProduct(int id, string name, string brand, string categoryName, decimal price, int contents, string unit);
        List<Category> GetCategories();
    }
}
