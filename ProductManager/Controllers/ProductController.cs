using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;

namespace ProductManager.MVC.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult ProductOverview()
        {
            IProductDAL productDAL = new ProductDAL(_configuration.GetConnectionString("ProductManagerTest")!);

            ProductService productLogic = new(productDAL);

            List<ProductViewModel> productViewModels = new();

            foreach (Product product in productLogic.GetProducts())
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    Name = product.Name,
                    Brand = product.Brand,
                    Category = product.Category.ToString()!,
                    Price = product.Price,
                    Contents = product.Contents,
                    Unit = product.Unit.ToString()!
                };

                productViewModels.Add(productViewModel);
            }

            return View(productViewModels);
        }

        [HttpGet]
        public IActionResult ProductOverview(IFormCollection formFields)
        {
            IProductDAL productDAL = new ProductDAL(_configuration.GetConnectionString("ProductManagerTest")!);

            ProductService productLogic = new(productDAL);

            List<ProductViewModel> productViewModels = new();

            foreach (Product product in productLogic.GetProducts())
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    Name = product.Name,
                    Brand = product.Brand,
                    Category = product.Category.ToString()!,
                    Price = product.Price,
                    Contents = product.Contents,
                    Unit = product.Unit.ToString()!
                };

                productViewModels.Add(productViewModel);
            }

            return View(productViewModels);
        }

        public IActionResult ProductCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreation(IFormCollection formFields)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ProductManagerTest")!);
            SqlCommand cmd = new SqlCommand("INSERT INTO Product ('Name', 'Brand', 'Price', 'Contents', 'Unit', '') VALUES ([Values])");

            string name = formFields["Name"].ToString();

            return View();
        }
    }
}
