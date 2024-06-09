using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface IOrderDAL
    {
        public List<Order> GetOrders();
        public Order GetOrder(int id);
        public string CreateOrder(Order order, List<OrderLine> orderLines);

        public List<Customer> GetCustomers();
        public List<Branch> GetBranches();
    }
}
