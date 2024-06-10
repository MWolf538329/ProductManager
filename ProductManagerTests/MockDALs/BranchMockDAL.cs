using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManagerTests.MockDALs
{
    public class BranchMockDAL : IBranchDAL
    {
        private readonly List<Branch> _branches =
            [
            new Branch() { ID = 1, Name = "PLUS Guido van Dijck", Address = "Kruidenlaan 159", PostalCode = "5803BV", City = "Venray"},
            new Branch() { ID = 2, Name = "AH Raadhuisstraat", Address = "Raadhuisstraat 100", PostalCode = "5801MA", City = "Venray" },

            new Branch() { ID = 0, Name = "Test", Address = "Test", PostalCode = "1234AB", City = "Test" }, // Invalid ID
            new Branch() { ID = 4, Name = "", Address = "Test", PostalCode = "1234AB", City = "Test" }, // Invalid Name
            new Branch() { ID = 5, Name = "Test", Address = "", PostalCode = "1234AB", City = "Test" }, // Invalid Address
            new Branch() { ID = 6, Name = "Test", Address = "Test", PostalCode = "", City = "Test" }, // Invalid PostalCode
            new Branch() { ID = 7, Name = "Test", Address = "Test", PostalCode = "1234ABC", City = "Test" }, // Invalid PostalCode - Exceeds Max Length
            new Branch() { ID = 8, Name = "Test", Address = "Test", PostalCode = "1234AB", City = "" } // Invalid City
            ];

        public List<Branch> GetBranches()
        {
            return _branches.Take(2).ToList();
        }

        public Branch GetBranch(int id)
        {
            return _branches.Where(b => b.ID == id).FirstOrDefault()!;
        }
        

        // Not Implemented
        public string CreateBranch(string name, string address, string postalcode, string city)
        {
            throw new NotImplementedException();
        }

        public string UpdateBranch(int id, string name, string address, string postalcode, string city)
        {
            throw new NotImplementedException();
        }
    }
}
