using Microsoft.AspNetCore.Mvc;

namespace ProductManager.Controllers
{
    public class BranchController : Controller
    {
        public IActionResult BranchOverview()
        {
            return View();
        }
    }
}
