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
    }
}
