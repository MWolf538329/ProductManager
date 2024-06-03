﻿using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;
using System.Runtime.Serialization.Json;

namespace ProductManager.DAL
{
    public class ProductDAL : IProductDAL
    {
        private string _conn;
        private SqlTransaction? _transaction;

        public ProductDAL(string conn)
        {
            _conn = conn;
        }

        public List<Product> GetProducts()
        {
            List<Product> products = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT P.ID, P.Name, P.Brand, P.Price, P.Contents, P.Unit, C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID", conn);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Product product = new Product();

                        product.ID = Convert.ToInt32(reader["ID"]);
                        product.Name = reader["Name"].ToString()!;
                        product.Brand = reader["Brand"].ToString()!;
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.Contents = Convert.ToInt32(reader["Contents"]);
                        product.Unit = Enum.Parse<Unit>(reader["Unit"].ToString()!);

                        if (!string.IsNullOrEmpty(reader["C.ID"].ToString()) && !string.IsNullOrEmpty(reader["C.Name"].ToString()))
                        {
                            Category category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! };
                            product.Category = category;
                        }

                        products.Add(product);
                    }
                }

                return products;
            }
        }

        public Product GetProduct(int id)
        {
            Product product = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new("SELECT P.ID, P.Name, P.Brand, P.Price, P.Contents, P.Unit, C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID " +
                    "WHERE P.ID = @productID");

                cmd.Parameters.AddWithValue("@productID", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        product.ID = Convert.ToInt32(reader["ID"]);
                        product.Name = reader["Name"].ToString()!;
                        product.Brand = reader["Brand"].ToString()!;
                        product.Price = Convert.ToDecimal(reader["Price"]);
                        product.Contents = Convert.ToInt32(reader["Contents"]);
                        product.Unit = Enum.Parse<Unit>(reader["Unit"].ToString()!);

                        if (!string.IsNullOrEmpty(reader["C.ID"].ToString()) && !string.IsNullOrEmpty(reader["C.Name"].ToString()))
                        {
                            Category category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! };
                            product.Category = category;
                        }
                    }
                }
            }

            return product;
        }

        public List<Category> GetCategories()
        {
            CategoryDAL categoryDAL = new(_conn);
            return categoryDAL.GetCategories();
        }

        //public List<Product> GetProducts(int skip, int take = 10)
        //{
        //    List<Product> products = new();

        //    using (SqlConnection conn = new SqlConnection(_conn))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID ORDER BY P.ID OFFSET @skip ROWS FETCH NEXT @take ROWS ONLY", conn);
        //        cmd.Parameters.AddWithValue("@skip", skip);
        //        cmd.Parameters.AddWithValue("@take", take);

        //        conn.Open();
        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                products.Add(new Product()
        //                {
        //                    ID = Convert.ToInt32(reader["P.ID"]),
        //                    Name = reader["P.Name"].ToString()!,
        //                    Brand = reader["P.Brand"].ToString()!,
        //                    Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! },
        //                    Price = Convert.ToDecimal(reader["P.Price"]),
        //                    Contents = Convert.ToInt32(reader["P.Contents"]),
        //                    Unit = (Unit)Enum.Parse(typeof(Unit), reader["P.Unit"].ToString()!)
        //                });
        //            }
        //        }
        //    }

        //    return products;
        //}

        //public List<Product> GetProducts(string name, string brand, string category, int skip, int take = 10)
        //{
        //    List<Product> products = new();

        //    using (SqlConnection conn = new SqlConnection(_conn))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID", conn);

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            products.Add(new Product()
        //            {
        //                ID = Convert.ToInt32(reader["P.ID"]),
        //                Name = reader["P.Name"].ToString()!,
        //                Brand = reader["P.Brand"].ToString()!,
        //                Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! },
        //                Price = Convert.ToDecimal(reader["P.Price"]),
        //                Contents = Convert.ToInt32(reader["P.Contents"]),
        //                Unit = (Unit)Enum.Parse(typeof(Unit), reader["P.Unit"].ToString()!)
        //            });
        //        }
        //    }

        //    return products;
        //}




        //public List<Category> GetCategories()
        //{
        //    List<Category> categories = new();

        //    using (SqlConnection conn = new SqlConnection(_conn))
        //    {
        //        SqlCommand cmd = new SqlCommand("SELECT ID, Name FROM Category", conn);
        //        cmd.Connection = conn;

        //        conn.Open();

        //        using (SqlDataReader reader = cmd.ExecuteReader())
        //        {
        //            while (reader.Read())
        //            {
        //                categories.Add(new Category()
        //                {
        //                    ID = Convert.ToInt32(reader["ID"]),
        //                    Name = reader["Name"].ToString()!,
        //                });
        //            }
        //        }
        //    }

        //    return categories;
        //}
    }
}
