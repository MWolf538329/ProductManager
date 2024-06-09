using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.Core
{
    public class OrderService
    {
        IOrderDAL _DAL;

        public OrderService(IOrderDAL dal)
        {
            _DAL = dal;
        }

        public IEnumerable<Order> GetOrders()
        {
            List<Order> orders = new();

            foreach (Order order in _DAL.GetOrders())
            {
                orders.Add(order);
            }

            return orders;
        }

        public Order GetOrder(int id)
        {
            return _DAL.GetOrder(id);
        }

        public List<Customer> GetCustomers()
        {
            return _DAL.GetCustomers();
        }

        public List<Branch> GetBranches()
        {
            return _DAL.GetBranches();
        }
    }
}
