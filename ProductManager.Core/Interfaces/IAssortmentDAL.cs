using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IAssortmentDAL
    {
        Assortment GetAssortment(int id);
        List<Product> GetProductsNotInAssortment(int id);
        string GetBranchName(int id);
        string AddProductToAssortmentOfBranch(int branchId, int productId);
        AssortmentProduct GetAssortmentProduct(int id);
        string UpdateAssortmentProductStock(int id, int stock);
        string DeleteProductFromAssortment(int id);
    }
}
