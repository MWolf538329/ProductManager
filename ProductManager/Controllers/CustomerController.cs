using Microsoft.AspNetCore.Mvc;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;
using ProductManager.MVC.Models;

namespace ProductManager.MVC.Controllers
{
    public class CustomerController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ICustomerDAL _customerDAL;
        private readonly CustomerService _customerService;

        public CustomerController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            //_connectionString = _configuration.GetConnectionString("ProductManagerTestLOCAL")!;
            _customerDAL = new CustomerDAL(_connectionString);
            _customerService = new(_customerDAL);
        }

        public IActionResult CustomerOverview()
        {
            List<CustomerViewModel> customerViewModels = new();

            foreach (Customer customer in _customerService.GetCustomers())
            {
                CustomerViewModel customerViewModel = new();

                customerViewModel.ID = customer.ID;
                customerViewModel.Name = customer.Name;
                customerViewModel.Address = customer.Address;
                customerViewModel.PostalCode = customer.PostalCode;
                customerViewModel.City = customer.City;

                customerViewModels.Add(customerViewModel);
            }

            ViewBag.SuccesMessage = TempData["SuccesMessage"];

            return View(customerViewModels);
        }

        public IActionResult CustomerCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CustomerCreation(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]) && !string.IsNullOrEmpty(formFields["Address"]) &&
                !string.IsNullOrEmpty(formFields["PostalCode"]) && !string.IsNullOrEmpty(formFields["City"]))
            {
                succesMessage = _customerService.CreateCustomer(formFields["Name"]!, formFields["Address"]!, formFields["PostalCode"]!, formFields["City"]!);
            }
            else succesMessage = "Customer input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CustomerOverview");
        }

        public IActionResult CustomerModification(int id)
        {
            CustomerViewModel customerViewModel = new();

            if (id != 0)
            {
                Customer customer = _customerService.GetCustomer(id);

                if (customer.ID != 0 && !string.IsNullOrEmpty(customer.Name) && !string.IsNullOrEmpty(customer.Address)
                    && !string.IsNullOrEmpty(customer.PostalCode) && !string.IsNullOrEmpty(customer.City))
                {
                    customerViewModel.ID = customer.ID;
                    customerViewModel.Name = customer.Name;
                    customerViewModel.Address = customer.Address;
                    customerViewModel.PostalCode = customer.PostalCode;
                    customerViewModel.City = customer.City;

                    return View(customerViewModel);
                }
            }

            return View();
        }

        [HttpPost]
        public IActionResult CustomerModification(IFormCollection formFields)
        {
            string succesMessage;

            if (!string.IsNullOrEmpty(formFields["Name"]) && !string.IsNullOrEmpty(formFields["Address"]) &&
                !string.IsNullOrEmpty(formFields["PostalCode"]) && !string.IsNullOrEmpty(formFields["City"]))
            {
                succesMessage = _customerService.UpdateCustomer(Convert.ToInt32(formFields["ID"]), formFields["Name"]!, formFields["Address"]!, formFields["PostalCode"]!, formFields["City"]!);
            }
            else succesMessage = "Customer input fields empty!";

            TempData["SuccesMessage"] = succesMessage;

            return RedirectToAction("CustomerOverview");
        }
    }
}
