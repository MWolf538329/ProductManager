using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.Core
{
    public class AssortmentService
    {
        readonly IAssortmentDAL _DAL;

        public AssortmentService(IAssortmentDAL dal)
        {
            _DAL = dal;
        }

        public Assortment GetAssortment(int id)
        {
            return _DAL.GetAssortment(id);
        }

        public AssortmentProduct GetAssortmentProduct(int id)
        {
            return _DAL.GetAssortmentProduct(id);
        }

        public List<Product> GetProductsNotInAssortment(int id)
        {
            return _DAL.GetProductsNotInAssortment(id);
        }

        public string AddProductToAssortmentOfBranch(int branchId, int productId)
        {
            return _DAL.AddProductToAssortmentOfBranch(branchId, productId);
        }

        public string GetBranchName(int id)
        {
            return _DAL.GetBranchName(id);
        }

        public string UpdateAssortmentProductStock(int id, int stock)
        {
            return _DAL.UpdateAssortmentProductStock(id, stock);
        }

        public string DeleteProductFromAssortment(int id)
        {
            return _DAL.DeleteProductFromAssortment(id);
        }
    }
}
