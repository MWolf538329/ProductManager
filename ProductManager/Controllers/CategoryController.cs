using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Interfaces;
using ProductManager.Core;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using ProductManager.Core.Models;

namespace ProductManager.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ICategoryDAL _categoryDAL;
        private readonly CategoryService _categoryService;

        public CategoryController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            _categoryDAL = new CategoryDAL(_connectionString);
            _categoryService = new(_categoryDAL);
        }

        public IActionResult CategoryOverview()
        {
            List<CategoryViewModel> categoryViewModels = new();

            foreach (Category category in _categoryService.GetCategories())
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
                succesMessage = _categoryService.CreateCategory(formFields["Name"]!);
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
                category = _categoryService.GetCategory(id);

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
                succesMessage = _categoryService.UpdateCategory(Convert.ToInt32(formFields["ID"]), formFields["Name"]!);
            }
            else succesMessage = "Category name input field empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
        }

        public IActionResult CategoryDeletion(int id)
        {
            string succesMessage;

            if (id != 0)
            {
                succesMessage = _categoryService.DeleteCategory(id);
            }
            else succesMessage = "Category can not be 0";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
        }
    }
}
