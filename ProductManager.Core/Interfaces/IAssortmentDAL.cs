using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IAssortmentDAL
    {
        Assortment GetAssortment(int id);
        List<Product> GetProductsNotInAssortment(int id);
        string GetBranchName(int id);
    }
}
