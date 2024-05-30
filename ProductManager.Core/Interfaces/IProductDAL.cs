using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IProductDAL
    {
        List<Product> GetProducts(int skip, int take = 10);
        List<Product> GetProducts(string name, string brand, string category, int skip, int take = 10);
    }
}
