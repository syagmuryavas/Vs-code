using HobbyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

namespace HobbyShop.Controllers
{
    public class AdminController : Controller
    {
        string connectionString = "Server=YAĞMUR\\SQLEXPRESS;Database=HobbyShopDB;Trusted_Connection=True;TrustServerCertificate=True;";

        public IActionResult AddProduct()
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand("SELECT * FROM Products", conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    products.Add(new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"].ToString(),
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        ImageUrl = reader["ImageUrl"].ToString()
                    });
                }
            }

            return View(products);
        }

        [HttpPost]
        public IActionResult AddProduct(string name,
           string description,
           decimal price,
           int stock,
           int categoryId,
           string imageUrl)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                SqlCommand cmd = new SqlCommand(
                "INSERT INTO Products (ProductName, Description, Price, Stock, CategoryID, ImageUrl) VALUES (@name, @desc, @price, @stock, @catId, @imageUrl)",
                conn);

                cmd.Parameters.AddWithValue("@name", name);
                cmd.Parameters.AddWithValue("@desc", description);
                cmd.Parameters.AddWithValue("@price", price);
                cmd.Parameters.AddWithValue("@stock", stock);
                cmd.Parameters.AddWithValue("@catId", categoryId);
                cmd.Parameters.AddWithValue("@imageUrl", imageUrl);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Products");
        }
        public IActionResult DeleteProduct(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Products WHERE ProductID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Products");
        }
        public IActionResult EditProduct(int id)
        {
            Product product = new Product();
            List<Category> categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Ürün çek
                string query = "SELECT * FROM Products WHERE ProductID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product.ProductID = (int)reader["ProductID"];
                    product.ProductName = reader["ProductName"]?.ToString() ?? "";
                    product.Description = reader["Description"]?.ToString() ?? "";
                    product.Price = (decimal)reader["Price"];
                    product.Stock = (int)reader["Stock"];
                    product.CategoryID = (int)reader["CategoryID"];
                }

                reader.Close();

                // Kategorileri çek
                string catQuery = "SELECT * FROM Categories";
                SqlCommand catCmd = new SqlCommand(catQuery, conn);
                SqlDataReader catReader = catCmd.ExecuteReader();

                while (catReader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = (int)catReader["CategoryID"],
                        CategoryName = catReader["CategoryName"]?.ToString() ?? ""
                    });
                }
            }

            ViewBag.Categories = categories;
            return View(product);
        }
        [HttpPost]
        public IActionResult EditProduct(Product product)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"UPDATE Products 
                         SET ProductName=@name, Description=@desc, Price=@price, Stock=@stock, CategoryID=@catId
                         WHERE ProductID=@id";

                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@name", product.ProductName);
                cmd.Parameters.AddWithValue("@desc", product.Description);
                cmd.Parameters.AddWithValue("@price", product.Price);
                cmd.Parameters.AddWithValue("@stock", product.Stock);
                cmd.Parameters.AddWithValue("@catId", product.CategoryID);
                cmd.Parameters.AddWithValue("@id", product.ProductID);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Index", "Products");
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var isAdmin = HttpContext.Session.GetString("IsAdmin");

            if (isAdmin != "true")
            {
                context.Result = RedirectToAction("Index", "Dashboard");
            }

            base.OnActionExecuting(context);
        }
        public IActionResult Orders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Orders";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    orders.Add(new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        OrderDate = (DateTime)reader["OrderDate"],
                        TotalPrice = (decimal)reader["TotalPrice"],
                        Status = reader["Status"]?.ToString() ?? ""
                    });
                }
            }

            return View(orders);
        }
        public IActionResult UpdateStatus(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = @"
                UPDATE Orders
                SET Status = 
                    CASE 
                        WHEN Status = 'Hazırlanıyor' THEN 'Kargoda'
                        WHEN Status = 'Kargoda' THEN 'Tamamlandı'
                        ELSE 'Hazırlanıyor'
                    END
                WHERE OrderID = @id";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Orders");
        }
        public IActionResult Categories()
        {
            List<Category> categories = new List<Category>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Categories";
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    categories.Add(new Category
                    {
                        CategoryID = (int)reader["CategoryID"],
                        CategoryName = reader["CategoryName"]?.ToString() ?? ""
                    });
                }
            }

            return View(categories);
        }
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddCategory(string categoryName)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Categories (CategoryName) VALUES (@name)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@name", categoryName);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Categories");
        }
        public IActionResult DeleteCategory(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "DELETE FROM Categories WHERE CategoryID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);
                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Categories");
        }
    }
}
