using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using ProductManager.DAL;

namespace ProductManager.Core
{
    public class AssortmentDAL : IAssortmentDAL
    {
        private readonly string _conn;
        private SqlTransaction _transaction;

        public AssortmentDAL(string conn)
        {
            _conn = conn;
        }

        public Assortment GetAssortment(int branchId)
        {
            Assortment assortment = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT A.ID AS 'A.ID', A.Stock AS 'A.Stock', P.ID AS 'P.ID', P.Name AS 'P.Name', " +
                    "P.Brand AS 'P.Brand', C.ID AS 'C.ID', C.Name AS 'C.Name', P.Price AS 'P.Price', P.Contents AS 'P.Contents', " +
                    "P.Unit AS 'P.Unit' " +
                    "FROM Assortment AS A " +
                    "LEFT JOIN Product AS P ON A.Product_ID = P.ID " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID " +
                    "WHERE A.Branch_ID = @branchID", conn);
                cmd.Parameters.AddWithValue("@branchID", branchId);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        AssortmentProduct assortmentProduct = new();

                        assortmentProduct.Id = Convert.ToInt32(reader["A.ID"]);

                        assortmentProduct.Product.ID = Convert.ToInt32(reader["P.ID"]);
                        assortmentProduct.Product.Name = reader["P.Name"].ToString()!;
                        assortmentProduct.Product.Brand = reader["P.Brand"].ToString()!;

                        if (!string.IsNullOrEmpty(reader["C.ID"].ToString()) && !string.IsNullOrEmpty(reader["C.Name"].ToString()))
                        {
                            assortmentProduct.Product.Category.ID = Convert.ToInt32(reader["C.ID"]);
                            assortmentProduct.Product.Category.Name = reader["C.Name"].ToString()!;
                        }
                        else assortmentProduct.Product.Category = new();
                        
                        assortmentProduct.Product.Price = Convert.ToDecimal(reader["P.Price"]);
                        assortmentProduct.Product.Contents = Convert.ToInt32(reader["P.Contents"]);
                        assortmentProduct.Product.Unit = Enum.Parse<Unit>(reader["P.Unit"].ToString()!);

                        assortmentProduct.Stock = Convert.ToInt32(reader["A.Stock"]);

                        assortment.AssortmentProducts.Add(assortmentProduct);
                    }
                }
            }

            return assortment;
        }

        public List<Product> GetProductsNotInAssortment(int id)
        {
            List<Product> products = new();

            using (SqlConnection conn = new(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', " +
                    "C.ID AS 'C.ID', C.Name AS 'C.Name', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit' " +
                    "FROM Product AS P " +
                    "LEFT JOIN Assortment AS A ON A.Product_ID = P.ID " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID " +
                    "WHERE A.ID IS NULL OR NOT A.Branch_ID = @branchId", conn);
                cmd.Parameters.AddWithValue("@branchId", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product newProduct = new();

                        newProduct.ID = Convert.ToInt32(reader["P.ID"]);
                        newProduct.Name = reader["P.Name"].ToString()!;
                        newProduct.Brand = reader["P.Brand"].ToString()!;

                        if (!string.IsNullOrEmpty(reader["C.ID"].ToString()!) && !string.IsNullOrEmpty(reader["C.Name"].ToString()!))
                        {
                            newProduct.Category.ID = Convert.ToInt32(reader["C.ID"]);
                            newProduct.Category.Name = reader["C.Name"].ToString()!;
                        }

                        newProduct.Price = Convert.ToDecimal(reader["P.Price"]);
                        newProduct.Contents = Convert.ToInt32(reader["P.Contents"]);
                        newProduct.Unit = Enum.Parse<Unit>(reader["P.Unit"].ToString()!);

                        products.Add(newProduct);
                    }
                }
            }

            return products;
        }

        public string GetBranchName(int id)
        {
            BranchDAL branchDAL = new(_conn);
            return branchDAL.GetBranchName(id);
        }

        
    }
}
