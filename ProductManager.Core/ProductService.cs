using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
namespace ProductManager.Core
{
    public class ProductService
    {
        IProductDAL _DAL;

        public ProductService(IProductDAL dal)
        {
            _DAL = dal;
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = new();

            foreach (Product product in _DAL.GetProducts())
            {
                Product newProduct = new Product()
                {
                    ID = product.ID,
                    Name = product.Name,
                    Brand = product.Brand,
                    Category = product.Category,
                    Price = product.Price,
                    Contents = product.Contents,
                    Unit = product.Unit
                };

                products.Add(newProduct);
            }
            return products;
        }

        public Product GetProduct(int id)
        {
            return _DAL.GetProduct(id);
        }

        public string CreateProduct(string name, string brand, string category, decimal price, int contents, string unit)
        {
            return string.Empty;
        }

        public string UpdateProduct(int id, string name, string brand, string category, decimal price, int contents, string unit)
        {
            return string.Empty;
        }

        public string DeleteProduct(int id)
        {
            return string.Empty;
        }


        public IEnumerable<Category> GetCategories()
        {
            List<Category> categories = new();

            foreach (Category category in _DAL.GetCategories())
            {
                Category newCategory = new Category()
                {
                    ID = category.ID,
                    Name = category.Name,
                };

                categories.Add(newCategory);
            }

            return categories;
        }

        
    }
}
