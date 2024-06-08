using ProductManager.Core.Models;

namespace ProductManager.Core.Interfaces
{
    public interface ICustomerDAL
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(int id);
        string CreateCustomer(string name, string address, string postalcode, string city);
        string UpdateCustomer(int id, string name, string address, string postalcode, string city);
    }
}
