using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.Core
{
    public class CustomerService
    {
        ICustomerDAL _DAL;

        public CustomerService(ICustomerDAL dal)
        {
            _DAL = dal;
        }

        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = new();

            foreach (Customer customer in _DAL.GetCustomers())
            {
                Customer newCustomer = new();

                newCustomer.ID = customer.ID;
                newCustomer.Name = customer.Name;
                newCustomer.Address = customer.Address;
                newCustomer.PostalCode = customer.PostalCode;
                newCustomer.City = customer.City;

                customers.Add(newCustomer);
            }

            return customers;
        }

        public Customer GetCustomer(int id)
        {
            return _DAL.GetCustomer(id);
        }

        public string CreateCustomer(string name, string address, string postalcode, string city)
        {
            return _DAL.CreateCustomer(name, address, postalcode, city);
        }

        public string UpdateCustomer(int id, string name, string address, string postalcode, string city)
        {
            return _DAL.UpdateCustomer(id, name, address, postalcode, city);
        }
    }
}
