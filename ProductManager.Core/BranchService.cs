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
            Branch branch = new();

            branch = _DAL.GetBranch(id);

            if (branch.ID == 0) throw new Exception("Branch ID can not be 0!");
            if (string.IsNullOrEmpty(branch.Name)) throw new Exception("Branch Name can not be empty!");
            if (string.IsNullOrEmpty(branch.Address)) throw new Exception("Branch Address can not be empty!");
            if (string.IsNullOrEmpty(branch.PostalCode)) throw new Exception("Branch PostalCode can not be empty!");
            if (branch.PostalCode.Length > 6) throw new Exception("Branch PostalCode can not be longer than 6 characters!");
            if (string.IsNullOrEmpty(branch.City)) throw new Exception("Branch City can not be empty!");

            return branch;
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
    }
}
