using Microsoft.Data.SqlClient;
using ProductManager.Core.Interfaces;
using ProductManager.Core.Models;

namespace ProductManager.DAL
{
    public class CategoryDAL : ICategoryDAL
    {
        private string _conn;

        public CategoryDAL(string conn)
        {
            _conn = conn;
        }

        public List<Category> GetCategories()
        {
            List<Category> categories = new();

            using (SqlConnection conn = new SqlConnection(_conn))
            {
                SqlCommand cmd = new SqlCommand("SELECT C_ID AS 'C_ID', C_Name AS 'C_Name' FROM Category AS 'C'", conn);

                conn.Open();

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(new Category()
                        {
                            ID = Convert.ToInt32(reader["C.ID"]),
                            Name = reader["C.Name"].ToString()!
                        });
                    }
                }
            }

            return categories;
        }
    }
}
