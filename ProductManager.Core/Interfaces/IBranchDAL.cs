using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IBranchDAL
    {
        List<Branch> GetBranches();
        Branch GetBranch(int id);
        string CreateBranch(string name, string address, string postalcode, string city);
        string UpdateBranch(int id, string name, string address, string postalcode, string city);
        string DeleteBranch(int id);
    }
}
