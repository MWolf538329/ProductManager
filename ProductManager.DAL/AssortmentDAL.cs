using Microsoft.Data.SqlClient;
using ProductManager.Core;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class AssortmentDAL : IAssortmentDAL
    {
        private readonly string _conn;
        private SqlTransaction? _transaction;

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
                        assortmentProduct.BranchID = branchId;

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
                SqlCommand cmd = new SqlCommand("SELECT DISTINCT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', " +
                    "C.ID AS 'C.ID', C.Name AS 'C.Name', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit' " +
                    "FROM Product AS P " +
                    "LEFT JOIN Assortment AS A ON A.Product_ID = P.ID " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID " +
                    "WHERE A.ID IS NULL OR(A.Branch_ID <> @branchId AND A.Product_ID NOT IN(SELECT Product_ID FROM Assortment WHERE Branch_ID = @branchId))", conn);
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

        public string AddProductToAssortmentOfBranch(int branchId, int productId)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Assortment (Product_ID, Branch_ID, Stock) VALUES (@productID, @branchID, 0)", conn);
                cmd.Parameters.AddWithValue("@branchID", branchId);
                cmd.Parameters.AddWithValue("@productID", productId);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Product succesfully added to assortment of branch!";
                }
                catch (SqlException sqlEx)
                {
                    _transaction.Rollback();
                    succesMessage = "Yet Unknown SQL Error!";
                    throw new Exception(sqlEx.Message);
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();
                    succesMessage = "Yet Unknown Error!";
                    throw new Exception(ex.Message);
                }
            }

            return succesMessage;
        }

        public AssortmentProduct GetAssortmentProduct(int id)
        {
            AssortmentProduct assortmentProduct = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT A.ID AS 'A.ID', P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', C.ID AS 'C.ID', C.Name AS 'C.Name', " +
                    "P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', A.Stock AS 'A.Stock', B.ID AS 'B.ID' FROM Assortment AS A " +
                    "INNER JOIN Product AS P ON A.Product_ID = P.ID " +
                    "LEFT JOIN Category AS C ON P.Category_ID = C.ID " +
                    "INNER JOIN Branch AS B ON A.Branch_ID = B.ID " +
                    "WHERE A.ID = @assortmentID", conn);
                cmd.Parameters.AddWithValue("@assortmentID", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        assortmentProduct.Id = Convert.ToInt32(reader["A.ID"]);
                        assortmentProduct.Product.ID = Convert.ToInt32(reader["P.ID"]);
                        assortmentProduct.Product.Name = reader["P.Name"].ToString()!;
                        assortmentProduct.Product.Brand = reader["P.Brand"].ToString()!;

                        if (!string.IsNullOrEmpty(reader["C.ID"].ToString()) && !string.IsNullOrEmpty(reader["C.Name"].ToString()))
                        {
                            assortmentProduct.Product.Category.ID = Convert.ToInt32(reader["C.ID"]);
                            assortmentProduct.Product.Category.Name = reader["C.Name"].ToString()!;
                        }
                        
                        assortmentProduct.Product.Price = Convert.ToDecimal(reader["P.Price"]);
                        assortmentProduct.Product.Contents = Convert.ToInt32(reader["P.Contents"]);
                        assortmentProduct.Product.Unit = Enum.Parse<Unit>(reader["P.Unit"].ToString()!);
                        assortmentProduct.Stock = Convert.ToInt32(reader["A.Stock"]);
                        assortmentProduct.BranchID = Convert.ToInt32(reader["B.ID"]);
                    }
                }
            }

            return assortmentProduct;
        }

        public string UpdateAssortmentProductStock(int id, int stock)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("UPDATE Assortment SET Stock = @stock WHERE ID = @id");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Assortment succesfully updated!";
                }
                catch (SqlException sqlEx)
                {
                    //// Number 2628 = Data Exceeds Field Max Length
                    //if (sqlEx.Number == 2628) succesMessage = "Product could not be updated because the inserted data would exceed the max length!";

                    //else
                    //{
                    _transaction.Rollback();
                    succesMessage = "Yet Unknown SQL Error!";
                    throw new Exception(sqlEx.Message);
                    //}
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

        public string DeleteProductFromAssortment(int id)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("DELETE FROM Assortment WHERE ID = @id");
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Assortment product succesfully deleted!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 547 = The DELETE statement conflicted with the REFERENCE constraint
                    //if (sqlEx.Number == 547) succesMessage = "Product could not be deleted because it is linked to 1 or more assortments!";

                    //else
                    //{
                    _transaction.Rollback();
                    succesMessage = "Yet Unknown SQL Error!";
                    throw new Exception(sqlEx.Message);
                    //}
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
