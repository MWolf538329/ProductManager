using Microsoft.AspNetCore.Mvc;

namespace ProductManager.MVC.Controllers
{
    public class BranchController : Controller
    {
        public IActionResult BranchOverview()
        {
            return View();
        }
    }
}
