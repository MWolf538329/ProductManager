using Microsoft.AspNetCore.Mvc;
using ProductManager.Core.Interfaces;
using ProductManager.Core;
using ProductManager.DAL;
using ProductManager.MVC.Models;
using ProductManager.Core.Models;

namespace ProductManager.MVC.Controllers
{
    public class OrderController : Controller
    {
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IOrderDAL _orderDAL;
        private readonly OrderService _orderService;

        public OrderController(IConfiguration configuration)
        {
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("ProductManagerTest")!;
            //_connectionString = _configuration.GetConnectionString("ProductManagerTestLOCAL")!;
            _orderDAL = new OrderDAL(_connectionString);
            _orderService = new(_orderDAL);
        }

        public IActionResult OrderOverview()
        {
            List<OrderViewModel> orderViewModels = new();

            foreach (Order order in _orderDAL.GetOrders())
            {
                OrderViewModel orderViewModel = new();

                orderViewModel.Id = order.ID;
                orderViewModel.CustomerName = order.Customer.Name;
                orderViewModel.BranchName = order.Branch.Name;

                orderViewModels.Add(orderViewModel);
            }

            return View(orderViewModels);
        }

        public IActionResult OrderDetails(int id)
        {
            OrderViewModel orderViewModel = new();
            List<OrderLineViewModel> orderLineViewModels = new();

            Order order = _orderDAL.GetOrder(id);

            orderViewModel.Id = order.ID;
            orderViewModel.CustomerName = order.Customer.Name;
            orderViewModel.BranchName = order.Branch.Name;

            foreach (OrderLine orderline in order.OrderLines)
            {
                OrderLineViewModel newOrderLine = new();

                newOrderLine.Id = orderline.ID;

                newOrderLine.Product.Id = orderline.Product.ID;
                newOrderLine.Product.Name = orderline.Product.Name;
                newOrderLine.Product.Brand = orderline.Product.Name;

                if (!string.IsNullOrEmpty(orderline.Product.Category.Name))
                {
                    newOrderLine.Product.CategoryName = orderline.Product.Category.Name;
                }
                
                newOrderLine.Product.Price = orderline.Product.Price;
                newOrderLine.Product.Contents = orderline.Product.Contents;
                newOrderLine.Product.Unit = orderline.Product.Unit.ToString();

                newOrderLine.Amount = orderline.Amount;

                orderLineViewModels.Add(newOrderLine);
            }

            orderViewModel.OrderLines = orderLineViewModels;

            return View(orderViewModel);
        }

        public IActionResult OrderSelection()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderSelection(IFormCollection formFields)
        {
            return View();
        }

        public IActionResult OrderCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult OrderCreation(IFormCollection formFields)
        {
            return View();
        }
    }
}
