using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class ProductDAL : IProductDAL
    {
        private readonly string _conn;
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

        public string CreateProduct(string name, string brand, string categoryName, decimal price, int contents, string unit)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Product (Name, Brand, Price, Contents, Unit, Category_ID) " +
                    "VALUES (@name, @brand, @price, @contents, @unit, (SELECT ID FROM Category WHERE Name = @categoryName))");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@contents", contents);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@categoryName", categoryName);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Product succesfully created!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2628 = Data Exceeds Field Max Length
                    if (sqlEx.Number == 2628) succesMessage = "Product could not be created because the inserted data would exceed the max length!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }

                    _transaction.Rollback();
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

        public string UpdateProduct(int id, string name, string brand, string categoryName, decimal price, int contents, string unit)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("UPDATE Product SET Name = @name, Brand = @brand, Price = @price, Contents = @contents, Unit = @unit, " +
                    "Category_ID = (SELECT ID FROM Category WHERE Name = @categoryName) WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@brand", brand);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@contents", contents);
                cmd.Parameters.AddWithValue("@unit", unit);
                cmd.Parameters.AddWithValue("@categoryName", categoryName);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Product succesfully updated!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2628 = Data Exceeds Field Max Length
                    if (sqlEx.Number == 2628) succesMessage = "Product could not be updated because the inserted data would exceed the max length!";

                    else
                    {
                        succesMessage = "Yet Unknown SQL Error!";
                        throw new Exception(sqlEx.Message);
                    }

                    _transaction.Rollback();
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

        public string DeleteProduct(int id)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("DELETE FROM Product WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Product succesfully deleted!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 547 = The DELETE statement conflicted with the REFERENCE constraint
                    if (sqlEx.Number == 547) succesMessage = "Product could not be deleted because it is linked to 1 or more assortments!";

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
