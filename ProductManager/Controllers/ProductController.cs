using Microsoft.AspNetCore.Mvc;
using ProductManager.Classes;
using Microsoft.Data.SqlClient;

namespace ProductManager.Controllers
{
    public class ProductController : Controller
    {
        private IConfiguration _configuration;

        public ProductController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult ProductOverview()
        {
            
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ProductManagerTest")!);
            SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name, P.Brand, P.Price, P.Contents, P.Unit, C.ID AS 'C.ID', C.Name FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<ProductBase> products = new List<ProductBase>();

            while (reader.Read())
            {
                products.Add(new ProductBase()
                {
                    ID = Convert.ToInt32(reader["P.ID"]),
                    Name = reader["Name"].ToString()!,
                    Brand = reader["Brand"].ToString()!,
                    Price = Convert.ToDecimal(reader["Price"]),
                    Contents = Convert.ToInt32(reader["Contents"]),
                    Unit = reader["Unit"].ToString()!,
                    Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["Name"].ToString()! }
                });
            }

            return View(products);
        }

        public IActionResult ProductCreation()
        {
            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            ProductDetailed productDetailed = new ProductDetailed();
            return View(productDetailed);
        }
    }
}
