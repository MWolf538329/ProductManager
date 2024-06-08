using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using System.Globalization;

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
                    Id = product.Id,
                    ProductID = product.Product.ID,
                    Name = product.Product.Name,
                    Brand = product.Product.Brand,
                    Category = product.Product.Category.Name,
                    Price = product.Product.Price,
                    Contents = product.Product.Contents,
                    Unit = product.Product.Unit.ToString(),
                    Stock = product.Stock,
                    BranchId = product.BranchID                    
                };
                assortmentViewModel.AssortmentProducts.Add(assortmentProduct);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];
            ViewBag.BranchName = branchName;
            ViewBag.BranchID = id;

            return View(assortmentViewModel);
        }

        public IActionResult AssortmentProductModification(int id)
        {
            AssortmentProductViewModel assortmentProductViewModel = new();

            if (id != 0)
            {
                AssortmentProduct assortmentProduct = new();

                assortmentProduct = _assortmentService.GetAssortmentProduct(id);

                assortmentProductViewModel.Id = assortmentProduct.Id;
                assortmentProductViewModel.ProductID = assortmentProduct.Product.ID;
                assortmentProductViewModel.Name = assortmentProduct.Product.Name;
                assortmentProductViewModel.Brand = assortmentProduct.Product.Brand;
                assortmentProductViewModel.Category = assortmentProduct.Product.Category.Name;
                assortmentProductViewModel.Price = assortmentProduct.Product.Price;
                assortmentProductViewModel.Contents = assortmentProduct.Product.Contents;
                assortmentProductViewModel.Unit = assortmentProduct.Product.Unit.ToString();
                assortmentProductViewModel.Stock = assortmentProduct.Stock;
                assortmentProductViewModel.BranchId = assortmentProduct.BranchID;

                return View(assortmentProductViewModel);
            }

            return View();
        }

        [HttpPost]
        public IActionResult AssortmentProductModification(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["ID"].ToString()) && !string.IsNullOrEmpty(formFields["Stock"].ToString()))
            {
                succesMessage = _assortmentService.UpdateAssortmentProductStock(Convert.ToInt32(formFields["ID"]), Convert.ToInt32(formFields["Stock"]));
            }
            else succesMessage = "Stock input field empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("AssortmentOverview", new { id = Convert.ToInt32(formFields["BranchID"]) });
        }

        public IActionResult AssortmentAddProduct(int id)
        {
            List<ProductViewModel> productViewModels = new();

            string branchName = _assortmentService.GetBranchName(id);

            foreach (Product product in _assortmentService.GetProductsNotInAssortment(id))
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
            ViewBag.BranchID = id;

            return View(productViewModels);
        }

        public IActionResult AddProductToAssortment(int branchId, int productId)
        {
            string succesMessage;

            succesMessage = _assortmentService.AddProductToAssortmentOfBranch(branchId, productId);

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("AssortmentAddProduct", new { id = branchId });
        }

        public IActionResult AssortmentProductDeletion(int branchId, int id)
        {
            string succesMessage;

            if (id != 0)
            {
                succesMessage = _assortmentService.DeleteProductFromAssortment(id);
            }
            else succesMessage = "Product can not be 0";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("AssortmentOverview", new { id = branchId });
        }
    }
}
