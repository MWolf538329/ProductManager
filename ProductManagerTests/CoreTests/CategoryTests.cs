using ProductManager.Core;
using ProductManager.Core.Models;
using ProductManagerTests.MockDALs;

namespace ProductManagerTests.CoreTests
{
    [TestClass]
    public class CategoryTests
    {
        private CategoryMockDAL categoryMockDAL { get; set; } = new();
        private CategoryService categoryServiceMock { get; set; }

        public CategoryTests()
        {
            categoryServiceMock = new(categoryMockDAL);
        }


        [TestMethod]
        public void GetCategoriesTest()
        {
            // Arrange
            List<Category> categories = new();

            // Act
            categories = categoryServiceMock.GetCategories().ToList();

            // Assert
            Assert.AreEqual(1, categories[0].ID);
            Assert.AreEqual("Test Category 1", categories[0].Name);

            Assert.AreEqual(2, categories[1].ID);
            Assert.AreEqual("Pasta", categories[1].Name);

            Assert.AreEqual(3, categories[2].ID);
            Assert.AreEqual("Schoonmaak middel", categories[2].Name);

            Assert.AreEqual(4, categories[3].ID);
            Assert.AreEqual("Frisdrank", categories[3].Name);

            Assert.AreEqual(5, categories[4].ID);
            Assert.AreEqual("Chips", categories[4].Name);

            Assert.AreEqual(6, categories[5].ID);
            Assert.AreEqual("Snoep", categories[5].Name);
        }

        [TestMethod]
        public void GetCategoryTest_Valid()
        {
            // Arrange
            Category category = new();

            // Act
            category = categoryServiceMock.GetCategory(2);

            // Arrange
            Assert.AreEqual(2, category.ID);
            Assert.AreEqual("Pasta", category.Name);
        }

        [TestMethod]
        public void GetCategoryTest_InvalidID()
        {
            // Act & Assert
            Assert.AreEqual("Category ID can not be 0!", Assert.ThrowsException<Exception>(
                () => categoryServiceMock.GetCategory(0)).Message);
        }

        [TestMethod]
        public void GetCategoryTest_InvalidName()
        {
            // Act & Assert
            Assert.AreEqual("Category Name can not be empty!", Assert.ThrowsException<Exception>(
                () => categoryServiceMock.GetCategory(8)).Message);
        }
    }
}