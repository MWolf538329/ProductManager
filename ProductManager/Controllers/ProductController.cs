using Microsoft.AspNetCore.Mvc;

namespace ProductManager.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult ProductOverview()
        {
            return View();
        }
    }
}
