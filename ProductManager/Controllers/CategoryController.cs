using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Interfaces;
using ProductManager.Core;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using ProductManager.Core.Models;
using Microsoft.Build.Framework;

namespace ProductManager.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private IConfiguration _configuration;
        private string connectionString;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
        }

        public IActionResult CategoryOverview()
        {
            ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
            CategoryService categoryService = new(categoryDAL);
            List<CategoryViewModel> categoryViewModels = new();

            foreach (Category category in categoryService.GetCategories())
            {
                CategoryViewModel categoryViewModel = new()
                {
                    Id = category.ID,
                    Name = category.Name
                };
                categoryViewModels.Add(categoryViewModel);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];

            return View(categoryViewModels);
        }

        public IActionResult CategoryCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CategoryCreation(IFormCollection formFields)
        {
            string message;

            if (!string.IsNullOrEmpty(formFields["Name"]))
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                if (categoryService.CreateCategory(formFields["Name"]!)) 
                    message = "Category succesfully created!"; else message = "Category could not be created!";
            }
            else message = "Category name input field empty!";

            TempData["SuccesMessage"] = message;

            return RedirectToAction("CategoryOverview", "Category", message);
        }

        public IActionResult CategoryModification(int id)
        {
            Category category = new();
            CategoryViewModel categoryViewModel = new();

            if (id != 0)
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                category = categoryService.GetCategory(id);

                if (category.ID != 0 && !string.IsNullOrEmpty(category.Name))
                {
                    categoryViewModel.Id = category.ID;
                    categoryViewModel.Name = category.Name;

                    return View(categoryViewModel);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult CategoryModification(IFormCollection formFields)
        {
            string message;

            if (!string.IsNullOrEmpty(formFields["Name"]))
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                if (categoryService.UpdateCategory(Convert.ToInt32(formFields["ID"]), formFields["Name"]!))
                    message = "Category succesfully updated!"; else message = "Category could not be updated!";
            }
            else message = "Category name input field empty!";

            TempData["SuccesMessage"] = message;

            return RedirectToAction("CategoryOverview");
        }

        public IActionResult CategoryDeletion(int id)
        {
            string message;

            if (id != 0)
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                if (categoryService.DeleteCategory(id)) message = "Category succesfully deleted!";
                else message = "Category could not be deleted!";
            }
            else message = "Category can not be 0";

            TempData["SuccesMessage"] = message;

            return RedirectToAction("CategoryOverview");
        }
    }
}
