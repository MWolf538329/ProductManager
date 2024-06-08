using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using System.ComponentModel;

namespace ProductManager.DAL
{
    public class OrderDAL : IOrderDAL
    {
        private readonly string _conn;
        private SqlTransaction? _transaction;

        public OrderDAL(string conn)
        {
            _conn = conn;
        } 

        public List<Order> GetOrders()
        {
            List<Order> orders = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT O.ID AS 'O.ID', B.ID AS 'B.ID', B.Name AS 'B.Name', B.Address AS 'B.Address', B.PostalCode AS 'B.PostalCode', " +
                    "B.City AS 'B.City', C.ID AS 'C.ID', C.Name AS 'C.Name', C.Address AS 'C.Address', C.PostalCode AS 'C.PostalCode', C.City AS 'C.City' " +
                    "FROM \"Order\" AS O " +
                    "INNER JOIN Branch AS B ON O.Branch_ID = B.ID " +
                    "INNER JOIN Customer AS C ON O.Customer_ID = C.ID", conn);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Order order = new();

                        order.ID = Convert.ToInt32(reader["O.ID"]);
                        
                        order.Branch.ID = Convert.ToInt32(reader["B.ID"]);
                        order.Branch.Name = reader["B.Name"].ToString()!;
                        order.Branch.Address = reader["B.Address"].ToString()!;
                        order.Branch.PostalCode = reader["B.PostalCode"].ToString()!;
                        order.Branch.City = reader["B.City"].ToString()!;

                        order.Customer.ID = Convert.ToInt32(reader["C.ID"]);
                        order.Customer.Name = reader["C.Name"].ToString()!;
                        order.Customer.Address = reader["C.Address"].ToString()!;
                        order.Customer.PostalCode = reader["C.PostalCode"].ToString()!;
                        order.Customer.City = reader["C.City"].ToString()!;

                        orders.Add(order);
                    }
                }
            }

            return orders;
        }

        public Order GetOrder(int id)
        {
            Order order = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT O.ID AS 'O.ID', B.ID AS 'B.ID', B.Name AS 'B.Name', B.Address AS 'B.Address', B.PostalCode AS 'B.PostalCode', B.City AS 'B.City', " +
                    "CU.ID AS 'CU.ID', CU.Name AS 'CU.Name', CU.Address AS 'CU.Address', CU.PostalCode AS 'CU.PostalCode', CU.City AS 'CU.City', P.ID AS 'P.ID', P.Name AS 'P.Name', " +
                    "P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', CA.ID AS 'CA.ID', CA.Name AS 'CA.Name', " +
                    "OL.ID AS 'OL.ID', OL.Amount AS 'OL.Amount' " +
                    "FROM \"Order\" AS O " +
                    "INNER JOIN OrderLine AS OL ON OL.Order_ID = O.ID " +
                    "INNER JOIN Branch AS B ON O.Branch_ID = B.ID " +
                    "INNER JOIN Customer AS CU ON O.Customer_ID = CU.ID " +
                    "INNER JOIN Product AS P ON OL.Product_ID = P.ID " +
                    "LEFT JOIN Category AS CA ON P.Category_ID = CA.ID " +
                    "WHERE O.ID = @orderId", conn);
                cmd.Parameters.AddWithValue("@orderId", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Setting Order ID ONCE
                        if (order.ID != Convert.ToInt32(reader["O.ID"])) order.ID = Convert.ToInt32(reader["O.ID"]);

                        // Setting Branch Properties ONCE
                        if (order.Branch.ID != Convert.ToInt32(reader["B.ID"])) order.Branch.ID = Convert.ToInt32(reader["B.ID"]);
                        if (order.Branch.Name != reader["B.Name"].ToString()!) order.Branch.Name = reader["B.Name"].ToString()!;
                        if (order.Branch.Address != reader["B.Address"].ToString()!) order.Branch.Address = reader["B.Address"].ToString()!;
                        if (order.Branch.PostalCode != reader["B.PostalCode"].ToString()!) order.Branch.PostalCode = reader["B.PostalCode"].ToString()!;
                        if (order.Branch.City != reader["B.City"].ToString()!) order.Branch.City = reader["B.City"].ToString()!;

                        // Setting Customer Properties ONCE
                        if (order.Customer.ID != Convert.ToInt32(reader["CU.ID"])) order.Customer.ID = Convert.ToInt32(reader["CU.ID"]);
                        if (order.Customer.Name != reader["CU.Name"].ToString()!) order.Customer.Name = reader["CU.Name"].ToString()!;
                        if (order.Customer.Address != reader["CU.Address"].ToString()!) order.Customer.Address = reader["CU.Address"].ToString()!;
                        if (order.Customer.PostalCode != reader["CU.PostalCode"].ToString()!) order.Customer.PostalCode = reader["CU.PostalCode"].ToString()!;
                        if (order.Customer.City != reader["CU.City"].ToString()!) order.Customer.City = reader["CU.City"].ToString()!;

                        OrderLine orderLine = new();

                        orderLine.ID = Convert.ToInt32(reader["OL.ID"]);
                        orderLine.Product.ID = Convert.ToInt32(reader["P.ID"]);
                        orderLine.Product.Name = reader["P.Name"].ToString()!;
                        orderLine.Product.Brand = reader["P.Brand"].ToString()!;

                        if (!string.IsNullOrEmpty(reader["CA.ID"].ToString()) && !string.IsNullOrEmpty(reader["CA.Name"].ToString()))
                        {
                            orderLine.Product.Category.ID = Convert.ToInt32(reader["CA.ID"]);
                            orderLine.Product.Category.Name = reader["CA.Name"].ToString()!;
                        }
                        
                        orderLine.Product.Price = Convert.ToDecimal(reader["P.Price"]);
                        orderLine.Product.Contents = Convert.ToInt32(reader["P.Contents"]);
                        orderLine.Product.Unit = Enum.Parse<Unit>(reader["P.Unit"].ToString()!);

                        orderLine.Amount = Convert.ToInt32(reader["OL.Amount"]);

                        order.OrderLines.Add(orderLine);
                    }
                }
            }

            return order;
        }

        public string CreateOrder(Order order, List<OrderLine> orderLines)
        {
            throw new NotImplementedException();
        }
    }
}
