using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private string _conn;
        private SqlTransaction? _transaction;

        public CategoryDAL(string conn)
        {
            _conn = conn;
        }

        public Category GetCategory(int id)
        {
            Category category = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new("SELECT Category.ID, Category.Name FROM Category WHERE Category.ID = @categoryID", conn);
                cmd.Parameters.AddWithValue("@categoryID", id);
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

        public List<Category> GetCategories()
        {
            List<Category> categories = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT Category.ID, Category.Name FROM Category", conn);

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

        public string CreateCategory(string name)
        {
            string succesMessage = string.Empty;

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                conn.Open();
                _transaction = conn.BeginTransaction();

                SqlCommand cmd = new SqlCommand("INSERT INTO Category (Name) VALUES (@categoryName)");
                cmd.Parameters.AddWithValue("@categoryName", name);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;
                
                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully created!";
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();

                    // Duplicate Category
                    if (ex.Message.Contains("Violation of UNIQUE KEY constraint 'Unique_Name'. Cannot insert duplicate key"))
                        succesMessage = "Category could not be created because one with the same name already exists!";

                    // Category Exceeds Max Length
                    if (ex.Message.Contains("String or binary data would be truncated in table"))
                        succesMessage = "Category could not be created because the name is too long!";
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

                SqlCommand cmd = new SqlCommand("UPDATE Category SET Category.Name = @categoryName WHERE Category.ID = @categoryID");
                cmd.Parameters.AddWithValue("@categoryName", name);
                cmd.Parameters.AddWithValue("@categoryID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully updated!";
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();

                    // Duplicate Category
                    if (ex.Message.Contains("Violation of UNIQUE KEY constraint 'Unique_Name'. Cannot insert duplicate key"))
                        succesMessage = "Category could not be updated because one with the same name already exists!";

                    // Category Exceeds Max Length
                    if (ex.Message.Contains("String or binary data would be truncated in table"))
                        succesMessage = "Category could not be updated because the name is too long!";
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

                SqlCommand cmd = new SqlCommand("DELETE FROM Category WHERE Category.ID = @categoryID");
                cmd.Parameters.AddWithValue("@categoryID", id);
                cmd.Connection = conn;
                cmd.Transaction = _transaction;

                try
                {
                    cmd.ExecuteNonQuery();
                    _transaction.Commit();
                    succesMessage = "Category succesfully deleted!";
                }
                catch (Exception ex)
                {
                    _transaction.Rollback();

                    // Category Linked To Product
                    if (ex.Message.Contains("The DELETE statement conflicted with the REFERENCE constraint"))
                        succesMessage = "Category could not be deleted because it is linked to 1 or more products!";
                }
            }
                
            return succesMessage;
        }
    }
}
