using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.DAL;

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

            productDAL.GetProducts(0);

            //return View(products);
            return View();
        }

        public IActionResult ProductCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreation(IFormCollection formFields)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ProductManagerTest")!);
            SqlCommand cmd = new SqlCommand("INSERT INTO Product ([Columns]) VALUES ([Values])");

            string name = formFields["Name"].ToString();

            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            //ProductDetailed productDetailed = new ProductDetailed();
            //return View(productDetailed);
            return View();
        }
    }
}
