using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;

namespace ProductManager.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IProductDAL _productDAL;
        private readonly ProductService _productService;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            _productDAL = new ProductDAL(_connectionString);
            _productService = new(_productDAL);
        }

        public IActionResult ProductOverview()
        {
            List<ProductViewModel> productViewModels = new();

            foreach (Product product in _productService.GetProducts())
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

            ViewBag.SuccesMessage = TempData["SuccesMessage"];

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
            List<string> categories = new List<string>();

            foreach (Category category in _productService.GetCategories())
            {
                categories.Add(category.Name);
            }

            ViewBag.Categories = categories;

            return View();
        }

        [HttpPost]
        public IActionResult ProductCreation(IFormCollection formFields)
        {
            string succesMessage;

            if (!InputEmpty(formFields["Name"].ToString()) && !InputEmpty(formFields["Brand"].ToString()) && !InputEmpty(formFields["Price"].ToString())
                && !InputEmpty(formFields["Contents"].ToString()) && !InputEmpty(formFields["Unit"].ToString()))
            {
                succesMessage = _productService.CreateProduct(formFields["Name"].ToString(), formFields["Brand"].ToString(),
                    formFields["CategoryName"].ToString(), Convert.ToDecimal(formFields["Price"]), Convert.ToInt32(formFields["Contents"]), formFields["Unit"].ToString());
            }
            else succesMessage = "Product input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("ProductOverview");
        }

        public IActionResult ProductModification(int id)
        {
            Product product = new();
            ProductViewModel productViewModel = new();

            if (id != 0)
            {
                product = _productService.GetProduct(id);

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

                    foreach (Category category in _productService.GetCategories())
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

            if (!InputEmpty(formFields["Name"].ToString()) && !InputEmpty(formFields["Brand"].ToString()) && !InputEmpty(formFields["Price"].ToString())
                && !InputEmpty(formFields["Contents"].ToString()) && !InputEmpty(formFields["Unit"].ToString()))
            {
                succesMessage = _productService.UpdateProduct(Convert.ToInt32(formFields["ID"]), formFields["Name"].ToString(), formFields["Brand"].ToString(), 
                    formFields["Category"].ToString(), Convert.ToDecimal(formFields["Price"]), Convert.ToInt32(formFields["Contents"]), formFields["Unit"].ToString());
            }
            else succesMessage = "Product input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("ProductOverview");
        }

        private bool InputEmpty(string input)
        {
            return string.IsNullOrEmpty(input); 
        }
    }
}
