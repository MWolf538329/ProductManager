using Microsoft.AspNetCore.Mvc;
using ProductManager.Classes;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

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
            SqlCommand cmd = new SqlCommand("SELECT P.ID AS 'P.ID', P.Name AS 'P.Name', P.Brand AS 'P.Brand', P.Price AS 'P.Price', P.Contents AS 'P.Contents', P.Unit AS 'P.Unit', C.ID AS 'C.ID', C.Name AS 'C.Name' FROM Product AS P INNER JOIN Category AS C ON P.Category_ID = C.ID", conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            List<ProductBase> products = new List<ProductBase>();

            while (reader.Read())
            {
                products.Add(new ProductBase()
                {
                    ID = Convert.ToInt32(reader["P.ID"]),
                    Name = reader["P.Name"].ToString()!,
                    Brand = reader["P.Brand"].ToString()!,
                    Price = Convert.ToDecimal(reader["P.Price"]),
                    Contents = Convert.ToInt32(reader["P.Contents"]),
                    Unit = reader["P.Unit"].ToString()!,
                    Category = new Category() { ID = Convert.ToInt32(reader["C.ID"]), Name = reader["C.Name"].ToString()! }
                });
            }

            return View(products);
        }

        public IActionResult ProductCreation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult ProductCreation(IFormCollection formFields)
        {
            SqlConnection conn = new SqlConnection(_configuration.GetConnectionString("ProductManagerTest")!);
            SqlCommand cmd = new SqlCommand("INSERT INTO Product ([Columns]) VALUES ([Values])");

            string name = formFields["Name"].ToString();

            return View();
        }

        public IActionResult ProductDetails(int id)
        {
            ProductDetailed productDetailed = new ProductDetailed();
            return View(productDetailed);
        }
    }
}
