using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private readonly string _conn;
        private SqlTransaction? _transaction;

        public CategoryDAL(string conn)
        {
            _conn = conn;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT ID, Name FROM Category", conn);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            ID = Convert.ToInt32(reader["ID"]),
                            Name = reader["Name"].ToString()!
                        });
                    }
                }
            }

            return categories;
        }

        public Category GetCategory(int id)
        {
            Category category = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new("SELECT ID, Name FROM Category WHERE ID = @ID", conn);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        category.ID = Convert.ToInt32(reader["ID"]);
                        category.Name = reader["Name"].ToString()!;
                    }
                }
            }

            return category;
        }

        public string CreateCategory(string name)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Category (Name) VALUES (@name)");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully created!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2627 = Violation of UNIQUE KEY constraint - Duplicate Item
                    if (sqlEx.Number == 2627) succesMessage = "Category could not be created because one with the same name already exists!";

                    // Number 2628 = Data Exceeds Field Max Length
                    else if (sqlEx.Number == 2628) succesMessage = "Category could not be created because the name is too long!";
                    
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

        public string UpdateCategory(int id, string name)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("UPDATE Category SET Name = @name WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully updated!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 2627 = Violation of UNIQUE KEY constraint - Duplicate Item
                    if (sqlEx.Number == 2627) succesMessage = "Category could not be created because one with the same name already exists!";

                    // Number 2628 = Data Exceeds Field Max Length
                    else if (sqlEx.Number == 2628) succesMessage = "Category could not be created because the name is too long!";

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

        public string DeleteCategory(int id)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE ID = @ID");
                cmd.Parameters.AddWithValue("@ID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully deleted!";
                }
                catch (SqlException sqlEx)
                {
                    // Number 547 = The DELETE statement conflicted with the REFERENCE constraint
                    if (sqlEx.Number == 547) succesMessage = "Category could not be deleted because it is linked to 1 or more products!";

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
