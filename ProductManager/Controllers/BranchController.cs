using Microsoft.AspNetCore.Mvc;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;

namespace ProductManager.MVC.Controllers
{
    public class BranchController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IBranchDAL _branchDAL;
        private readonly BranchService _branchService;

        public BranchController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            _branchDAL = new BranchDAL(_connectionString);
            _branchService = new(_branchDAL);
        }

        public IActionResult BranchOverview()
        {
            List<BranchViewModel> branchViewModels = new();
            
            foreach (Branch branch in _branchService.GetBranches())
            {
                BranchViewModel branchViewModel = new()
                {
                    Id = branch.ID,
                    Name = branch.Name,
                    Address = branch.Address,
                    PostalCode = branch.PostalCode,
                    City = branch.City
                };
                branchViewModels.Add(branchViewModel);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];

            return View(branchViewModels);
        }

        public IActionResult BranchCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult BranchCreation(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]) && !string.IsNullOrEmpty(formFields["Address"])
                && !string.IsNullOrEmpty(formFields["PostalCode"]) && !string.IsNullOrEmpty(formFields["City"]))
            {
                succesMessage = _branchService.CreateBranch(formFields["Name"]!, formFields["Address"]!,
                    formFields["PostalCode"].ToString().ToUpper()!, formFields["City"]!);
            }
            else succesMessage = "Branch input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("BranchOverview");
        }

        public IActionResult BranchModification(int id)
        {
            BranchViewModel branchViewModel = new();

            if (id != 0)
            {
                Branch branch = _branchService.GetBranch(id);

                if (branch.ID != 0 && !string.IsNullOrEmpty(branch.Name) && !string.IsNullOrEmpty(branch.Address)
                    && !string.IsNullOrEmpty(branch.PostalCode) && !string.IsNullOrEmpty(branch.City))
                {
                    branchViewModel.Id = branch.ID;
                    branchViewModel.Name = branch.Name;
                    branchViewModel.Address = branch.Address;
                    branchViewModel.PostalCode = branch.PostalCode;
                    branchViewModel.City = branch.City;

                    return View(branchViewModel);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult BranchModification(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]) && !string.IsNullOrEmpty(formFields["Address"])
                && !string.IsNullOrEmpty(formFields["PostalCode"].ToString().ToUpper()) && !string.IsNullOrEmpty(formFields["City"]))
            {
                succesMessage = _branchService.UpdateBranch(Convert.ToInt32(formFields["ID"]), formFields["Name"]!, formFields["Address"]!
                    , formFields["PostalCode"]!, formFields["City"]!);
            }
            else succesMessage = "Branch input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("BranchOverview");
        }

        public IActionResult BranchDeletion(int id)
        {
            string succesMessage;

            if (id != 0)
            {
                succesMessage = _branchService.DeleteBranch(id);
            }
            else succesMessage = "Branch can not be 0";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("BranchOverview");
        }
    }
}
