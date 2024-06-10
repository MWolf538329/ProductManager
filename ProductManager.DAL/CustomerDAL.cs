using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class CustomerDAL : ICustomerDAL
    {
        private readonly string _conn;
        private SqlTransaction? _transaction;

        public CustomerDAL(string conn)
        {
            _conn = conn;
        }

        public List<Customer> GetCustomers()
        {
            List<Customer> customers = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Name, Address, PostalCode, City FROM Customer", conn);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Customer customer = new();

                        customer.ID = Convert.ToInt32(reader["ID"]);
                        customer.Name = reader["Name"].ToString()!;
                        customer.Address = reader["Address"].ToString()!;
                        customer.PostalCode = reader["PostalCode"].ToString()!;
                        customer.City = reader["City"].ToString()!;

                        customers.Add(customer);
                    }
                }
            }

            return customers;
        }

        public Customer GetCustomer(int id)
        {
            Customer customer = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Name, Address, PostalCode, City FROM Customer WHERE ID = @id", conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        customer.ID = Convert.ToInt32(reader["ID"]);
                        customer.Name = reader["Name"].ToString()!;
                        customer.Address = reader["Address"].ToString()!;
                        customer.PostalCode = reader["PostalCode"].ToString()!;
                        customer.City = reader["City"].ToString()!;
                    }
                }
            }

            return customer;
        }

        public string CreateCustomer(string name, string address, string postalcode, string city)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Customer (Name, Address, PostalCode, City) VALUES (@name, @address, @postalcode, @city)");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@postalcode", postalcode);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Customer succesfully created!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2627 = Violation of UNIQUE KEY constraint - Duplicate Item
                    if (sqlEx.Number == 2627) succesMessage = "Customer could not be created because one with the same name already exists!";

                    // Number 2628 = Data Exceeds Field Max Length
                    else if (sqlEx.Number == 2628) succesMessage = "Customer could not be created because data in the field is too long!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }

        public string UpdateCustomer(int id, string name, string address, string postalcode, string city)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("UPDATE Customer SET Name = @name, Address = @address, PostalCode = @postalcode, City = @city WHERE ID = @id");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@address", address);
                cmd.Parameters.AddWithValue("@postalcode", postalcode);
                cmd.Parameters.AddWithValue("@city", city);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Customer succesfully updated!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2627 = Violation of UNIQUE KEY constraint - Duplicate Item
                    if (sqlEx.Number == 2627) succesMessage = "Customer could not be updated because one with the same name already exists!";

                    // Number 2628 = Data Exceeds Field Max Length
                    else if (sqlEx.Number == 2628) succesMessage = "Customer could not be updated because data in the field is too long!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }
    }
}
