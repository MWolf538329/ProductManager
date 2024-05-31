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

        public bool CreateCategory(string name)
        {
            bool succes;

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
                    succes = true;
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    succes = false;
                }
            }

            return succes;
        }

        public bool UpdateCategory(int id, string name)
        {
            bool succes;

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
                    succes = true;
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    succes = false;
                }
            }
                
            return succes;
        }

        public bool DeleteCategory(int id)
        {
            bool succes;

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
                    succes = true;
                }
                catch (Exception)
                {
                    _transaction.Rollback();
                    succes = false;
                }
            }
                
            return succes;
        }
    }
}
