using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
namespace ProductManager.Core
{
    public class ProductLogic
    {
        IProductDAL _DAL;

        public ProductLogic(IProductDAL dal)
        {
            _DAL = dal;
        }

        public IEnumerable<Product> GetProducts()
        {
            List<Product> products = new();

            foreach (Product product in _DAL.GetProducts(0))
            {
                Product newProduct = new Product()
                {
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
    }
}
