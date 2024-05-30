using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Interfaces;
using ProductManager.Core;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using ProductManager.Core.Models;
using Microsoft.Data.SqlClient;

namespace ProductManager.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private IConfiguration _configuration;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult CategoryOverview()
        {
            ICategoryDAL categoryDAL = new CategoryDAL(_configuration.GetConnectionString("ProductManagerTest")!);
            CategoryLogic categoryLogic = new(categoryDAL);
            List<CategoryViewModel> categoryViewModels = new();

            foreach (Category category in categoryLogic.GetCategories())
            {
                CategoryViewModel categoryViewModel = new CategoryViewModel()
                {
                    Id = category.ID,
                    Name = category.Name
                };
                categoryViewModels.Add(categoryViewModel);
            }

            return View(categoryViewModels);
        }

        public IActionResult CategoryCreation() 
        {
            return View();     
        }

        [HttpPost]
        public IActionResult CategoryCreation(IFormCollection formFields) 
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ProductManagerTest")!);
            SqlCommand cmd = new SqlCommand("INSERT INTO Category ('Name') VALUES (@categoryName)");
            cmd.Parameters.AddWithValue("@categoryName", formFields["Name"].ToString());
            cmd.Connection = conn;

            conn.Open();

            cmd.ExecuteNonQuery();

            return View();
        }
    }
}
