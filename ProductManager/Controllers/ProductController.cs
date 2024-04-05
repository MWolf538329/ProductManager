using Microsoft.AspNetCore.Mvc;
using ProductManager.Classes;

namespace ProductManager.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductOverview()
        {
            return View();
        }

        public IActionResult ProductCreation()
        {
            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            ProductDetailed productDetailed = new ProductDetailed();
            return View(productDetailed);
        }
    }
}
