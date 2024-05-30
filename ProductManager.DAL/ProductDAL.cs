using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class ProductDAL : IProductDAL
    {
        string _conn;

        public ProductDAL(string conn)
        {
            _conn = conn;
        }

        public List<Product> GetProducts(int skip, int take = 10)
        {
            List<Product> products = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID ORDER BY P.ID OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", conn);
                cmd.Parameters.AddWithValue("@skip", skip);
                cmd.Parameters.AddWithValue("@take", take);

                conn.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        products.Add(new Product()
                        {
                            ID = Convert.ToInt32(reader["P.ID"]),
                            Name = reader["P.Name"].ToString()!,
                            Brand = reader["P.Brand"].ToString()!,
                            Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! },
                            Price = Convert.ToDecimal(reader["P.Price"]),
                            Contents = Convert.ToInt32(reader["P.Contents"]),
                            Unit = (Unit)Enum.Parse(typeof(Unit), reader["P.Unit"].ToString()!)
                        });
                    }
                }
            }

            return products;
        }

        public List<Product> GetProducts(string name, string brand, string category, int skip, int take = 10)
        {
            List<Product> products = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID", conn);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    products.Add(new Product()
                    {
                        ID = Convert.ToInt32(reader["P.ID"]),
                        Name = reader["P.Name"].ToString()!,
                        Brand = reader["P.Brand"].ToString()!,
                        Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! },
                        Price = Convert.ToDecimal(reader["P.Price"]),
                        Contents = Convert.ToInt32(reader["P.Contents"]),
                        Unit = (Unit)Enum.Parse(typeof(Unit), reader["P.Unit"].ToString()!)
                    });
                }
            }

            return products;
        }
    }
}
