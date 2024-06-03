using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Interfaces;
using ProductManager.Core;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using ProductManager.Core.Models;
using Microsoft.Build.Framework;
using Microsoft.AspNetCore.Components.Forms;

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
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]))
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                succesMessage = categoryService.CreateCategory(formFields["Name"]!);
            }
            else succesMessage = "Category name input field empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
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
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]))
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                succesMessage = categoryService.UpdateCategory(Convert.ToInt32(formFields["ID"]), formFields["Name"]!);
            }
            else succesMessage = "Category name input field empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
        }

        [HttpPost]
        public IActionResult CategoryDeletion(int id)
        {
            string succesMessage;

            if (id != 0)
            {
                ICategoryDAL categoryDAL = new CategoryDAL(connectionString);
                CategoryService categoryService = new(categoryDAL);

                succesMessage = categoryService.DeleteCategory(id);
            }
            else succesMessage = "Category can not be 0";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
        }
    }
}
