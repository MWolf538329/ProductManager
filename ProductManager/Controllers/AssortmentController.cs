using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.View;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using System.Configuration;

namespace ProductManager.MVC.Controllers
{
    public class AssortmentController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IAssortmentDAL _assortmentDAL;
        private readonly AssortmentService _assortmentService;

        public AssortmentController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            //_connectionString = _configuration.GetConnectionString("ProductManagerTestLOCAL")!;
            _assortmentDAL = new AssortmentDAL(_connectionString);
            _assortmentService = new(_assortmentDAL);
        }

        public IActionResult AssortmentOverview(int id)
        {
            AssortmentViewModel assortmentViewModel = new();
            Assortment assortment = new();

            string branchName = _assortmentDAL.GetBranchName(id);

            assortment = _assortmentService.GetAssortment(id);

            foreach (AssortmentProduct product in assortment.AssortmentProducts)
            {
                AssortmentProductViewModel assortmentProduct = new()
                {
                    Id = product.Product.ID,
                    Name = product.Product.Name,
                    Brand = product.Product.Brand,
                    Category = product.Product.Category.Name,
                    Price = product.Product.Price,
                    Contents = product.Product.Contents,
                    Unit = product.Product.Unit.ToString(),
                    Stock = product.Stock
                };
                assortmentViewModel.AssortmentProducts.Add(assortmentProduct);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];
            ViewBag.BranchName = branchName;
            ViewBag.BranchID = id;

            return View(assortmentViewModel);
        }

        public IActionResult AssortmentAddProduct(int id)
        {
            List<ProductViewModel> productViewModels = new();

            string branchName = _assortmentDAL.GetBranchName(id);

            foreach (Product product in _assortmentDAL.GetProductsNotInAssortment(id))
            {
                ProductViewModel productViewModel = new();

                productViewModel.Id = product.ID;
                productViewModel.Name = product.Name;
                productViewModel.Brand = product.Brand;

                if (!string.IsNullOrEmpty(product.Category.ID.ToString()) && !string.IsNullOrEmpty(product.Category.Name))
                {
                    productViewModel.CategoryName = product.Category.Name;
                }

                productViewModel.Price = product.Price;
                productViewModel.Contents = product.Contents;
                productViewModel.Unit = product.Unit.ToString();

                productViewModels.Add(productViewModel);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];
            ViewBag.BranchName = branchName;

            return View(productViewModels);
        }

        //public IActionResult AddProductToAssortment(int id)
        //{

        //}

    }
}
