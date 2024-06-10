using ProductManager.Core;
using ProductManager.Core.Models;
using ProductManagerTests.MockDALs;

namespace ProductManagerTests.CoreTests
{
    [TestClass]
    public class BranchTests
    {
        private readonly BranchMockDAL branchMockDAL = new();
        private readonly BranchService _branchService;

        public BranchTests()
        {
            _branchService = new(branchMockDAL);
        }


        [TestMethod]
        public void GetBranches_Valid()
        {
            // Arrange
            List<Branch> branches = new();

            // Act
            branches = _branchService.GetBranches().ToList();

            // Assert
            Assert.AreEqual(1, branches[0].ID);
            Assert.AreEqual("PLUS Guido van Dijck", branches[0].Name);
            Assert.AreEqual("Kruidenlaan 159", branches[0].Address);
            Assert.AreEqual("5803BV", branches[0].PostalCode);
            Assert.AreEqual("Venray", branches[0].City);

            Assert.AreEqual(2, branches[1].ID);
            Assert.AreEqual("AH Raadhuisstraat", branches[1].Name);
            Assert.AreEqual("Raadhuisstraat 100", branches[1].Address);
            Assert.AreEqual("5801MA", branches[1].PostalCode);
            Assert.AreEqual("Venray", branches[1].City);
        }

        [TestMethod]
        public void GetBranch_Valid()
        {
            // Arrange
            Branch branch = new();

            // Act
            branch = _branchService.GetBranch(1);

            // Assert
            Assert.AreEqual(1, branch.ID);
            Assert.AreEqual("PLUS Guido van Dijck", branch.Name);
            Assert.AreEqual("Kruidenlaan 159", branch.Address);
            Assert.AreEqual("5803BV", branch.PostalCode);
            Assert.AreEqual("Venray", branch.City);
        }

        [TestMethod]
        public void GetBranch_InvalidID()
        {
            // Act & Assert
            Assert.AreEqual("Branch ID can not be 0!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(0)).Message);
        }

        [TestMethod]
        public void GetBranch_InvalidName()
        {
            // Act & Assert
            Assert.AreEqual("Branch Name can not be empty!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(4)).Message);
        }

        [TestMethod]
        public void GetBranch_InvalidAddress()
        {
            // Act & Assert
            Assert.AreEqual("Branch Address can not be empty!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(5)).Message);
        }

        [TestMethod]
        public void GetBranch_InvalidPostalCode()
        {
            // Act & Assert
            Assert.AreEqual("Branch PostalCode can not be empty!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(6)).Message);
        }

        [TestMethod]
        public void GetBranch_InvalidPostalCode_ExceedMaxLength()
        {
            // Act & Assert
            Assert.AreEqual("Branch PostalCode can not be longer than 6 characters!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(7)).Message);
        }

        [TestMethod]
        public void GetBranch_InvalidCity()
        {
            // Act & Assert
            Assert.AreEqual("Branch City can not be empty!", Assert.ThrowsException<Exception>(
                () => _branchService.GetBranch(8)).Message);
        }
    }
}
