using Microsoft.AspNetCore.Mvc;
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
        private string connectionString;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
        }

        public IActionResult ProductOverview()
        {
            IProductDAL productDAL = new ProductDAL(connectionString);

            ProductService productService = new(productDAL);

            List<ProductViewModel> productViewModels = new();

            foreach (Product product in productService.GetProducts())
            {
                ProductViewModel productViewModel = new ProductViewModel()
                {
                    Id = product.ID,
                    Name = product.Name,
                    Brand = product.Brand,
                    CategoryName = product.Category.Name,
                    Price = product.Price,
                    Contents = product.Contents,
                    Unit = product.Unit.ToString()!
                };

                productViewModels.Add(productViewModel);
            }

            return View(productViewModels);
        }

        //[HttpGet]
        //public IActionResult ProductOverview(IFormCollection formFields)
        //{
        //    IProductDAL productDAL = new ProductDAL(_configuration.GetConnectionString("ProductManagerTest")!);

        //    ProductService productService = new(productDAL);

        //    List<ProductViewModel> productViewModels = new();

        //    foreach (Product product in productService.GetProducts())
        //    {
        //        ProductViewModel productViewModel = new ProductViewModel()
        //        {
        //            Name = product.Name,
        //            Brand = product.Brand,
        //            Category = product.Category.ToString()!,
        //            Price = product.Price,
        //            Contents = product.Contents,
        //            Unit = product.Unit.ToString()!
        //        };

        //        productViewModels.Add(productViewModel);
        //    }

        //    return View(productViewModels);
        //}

        public IActionResult ProductCreation()
        {
            IProductDAL productDAL = new ProductDAL(connectionString);

            ProductService productService = new(productDAL);

            List<string> categories = new List<string>();

            foreach (Category category in productService.GetCategories())
            {
                categories.Add(category.Name);
            }

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public IActionResult ProductCreation(IFormCollection formFields)
        {
            IProductDAL productDAL = new ProductDAL(connectionString);

            ProductService productService = new(productDAL);

            //List<string> categories = new List<string>();

            //foreach (Category category in productService.GetCategories())
            //{
            //    categories.Add(category.Name);
            //}

            //TempData["Categories"] = categories;

            return View();
        }

        public IActionResult ProductModification(int id)
        {
            Product product = new();
            ProductViewModel productViewModel = new();

            if (id != 0)
            {
                IProductDAL productDAL = new ProductDAL(connectionString);
                ProductService productService = new(productDAL);

                product = productService.GetProduct(id);

                if (product.ID != 0 && !string.IsNullOrEmpty(product.Name))
                {
                    productViewModel.Id = product.ID;
                    productViewModel.Name = product.Name;
                    productViewModel.Brand = product.Brand;
                    productViewModel.Price = product.Price;
                    productViewModel.Contents = product.Contents;
                    productViewModel.Unit = product.Unit.ToString();

                    if (product.Category.ID != 0 && !string.IsNullOrEmpty(product.Category.Name))
                    {
                        productViewModel.CategoryName = product.Category.Name;
                    }

                    List<string> categories = new List<string>();

                    foreach (Category category in productService.GetCategories())
                    {
                        categories.Add(category.Name);
                    }

                    ViewBag.Categories = categories;

                    return View(productViewModel);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult ProductModification(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]))
            {
                IProductDAL productDAL = new ProductDAL(connectionString);
                ProductService productService = new(productDAL);

                succesMessage = productService.UpdateProduct();
                //succesMessage = productService.UpdateProduct(Convert.ToInt32(formFields["ID"]), formFields["Name"]!);
            }
            else succesMessage = "Category name input field empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CategoryOverview");
        }
    }
}
