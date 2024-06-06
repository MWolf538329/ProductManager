using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.Core
{
    public class BranchService
    {
        readonly IBranchDAL _DAL;

        public BranchService(IBranchDAL dal)
        {
            _DAL = dal;
        }

        public Branch GetBranch(int id)
        {
            return _DAL.GetBranch(id);
        }

        public IEnumerable<Branch> GetBranches()
        {
            List<Branch> branches = new();

            foreach (Branch branch in _DAL.GetBranches())
            {
                Branch newBranch = new()
                {
                    ID = branch.ID,
                    Name = branch.Name,
                    Address = branch.Address,
                    PostalCode = branch.PostalCode,
                    City = branch.City
                };

                branches.Add(newBranch);
            }

            return branches;
        }

        public string CreateBranch(string name, string address, string postalcode, string city)
        {
            return _DAL.CreateBranch(name, address, postalcode, city);
        }

        public string UpdateBranch(int id, string name, string address, string postalcode, string city)
        {
            return _DAL.UpdateBranch(id, name, address, postalcode, city);
        }

        public string DeleteBranch(int id)
        {
            return _DAL.DeleteBranch(id);
        }
    }
}
