using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManagerTests.MockDALs
{
    public class CategoryMockDAL : ICategoryDAL
    {
        private readonly List<Category> _categories = 
            [
                new() { ID = 1, Name = "Test Category 1" },
                new() { ID = 2, Name = "Pasta" },
                new() { ID = 3, Name = "Schoonmaak middel" },
                new() { ID = 4, Name = "Frisdrank" },
                new() { ID = 5, Name = "Chips" },
                new() { ID = 6, Name = "Snoep" },
                new() { ID = 0, Name = "Test" },
                new() { ID = 8, Name = "" }
            ];

        public List<Category> GetCategories()
        {
            return _categories.Take(6).ToList();
        }

        public Category GetCategory(int id)
        {
            return _categories.Where(c => c.ID == id).FirstOrDefault()!;
        }



        // Not Implemented
        public string CreateCategory(string name)
        {
            throw new NotImplementedException();
        }

        public string UpdateCategory(int id, string name)
        {
            throw new NotImplementedException();
        }

        public string DeleteCategory(int id)
        {
            throw new NotImplementedException();
        }
    }
}
