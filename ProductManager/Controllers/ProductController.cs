﻿using Microsoft.AspNetCore.Mvc;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using System.Globalization;

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
            //_connectionString = _configuration.GetConnectionString("ProductManagerTestLOCAL")!;
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

            if (!string.IsNullOrEmpty(formFields["Name"].ToString()) && !string.IsNullOrEmpty(formFields["Brand"].ToString()) && !string.IsNullOrEmpty(formFields["Price"].ToString())
                && !string.IsNullOrEmpty(formFields["Contents"].ToString()) && !string.IsNullOrEmpty(formFields["Unit"].ToString()))
            {
                succesMessage = _productService.CreateProduct(formFields["Name"].ToString(), formFields["Brand"].ToString(),
                    formFields["CategoryName"].ToString(), Convert.ToDecimal(formFields["Price"], CultureInfo.CreateSpecificCulture("en-US")),
                    Convert.ToInt32(formFields["Contents"]), formFields["Unit"].ToString());
            }
            else succesMessage = "Product input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("ProductOverview");
        }

        public IActionResult ProductModification(int id)
        {
            ProductViewModel productViewModel = new();

            if (id != 0)
            {
                Product product = _productService.GetProduct(id);

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

            if (!string.IsNullOrEmpty(formFields["Name"].ToString()) && !string.IsNullOrEmpty(formFields["Brand"].ToString()) && !string.IsNullOrEmpty(formFields["Price"].ToString())
                && !string.IsNullOrEmpty(formFields["Contents"].ToString()) && !string.IsNullOrEmpty(formFields["Unit"].ToString()))
            {
                succesMessage = _productService.UpdateProduct(Convert.ToInt32(formFields["ID"]), formFields["Name"].ToString(), formFields["Brand"].ToString(), 
                    formFields["CategoryName"].ToString(), Convert.ToDecimal(formFields["Price"], CultureInfo.CreateSpecificCulture("en-US")),
                    Convert.ToInt32(formFields["Contents"]), formFields["Unit"].ToString());
            }
            else succesMessage = "Product input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("ProductOverview");
        }

        public IActionResult ProductDeletion(int id)
        {
            string succesMessage;

            if (id != 0)
            {
                succesMessage = _productService.DeleteProduct(id);
            }
            else succesMessage = "Product can not be 0";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("ProductOverview");
        }
    }
}
