using HobbyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace HobbyShop.Controllers
{
    public class ProductsController : Controller
    {
        private string connectionString = "Server=YAĞMUR\\SQLEXPRESS;Database=HobbyShopDB;Trusted_Connection=True;TrustServerCertificate=True;";

        // ÜRÜN LİSTELEME
        public IActionResult Index(int? categoryId)
        {
            List<Product> products = new List<Product>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query;

                if (categoryId != null)
                {
                    query = "SELECT * FROM Products WHERE CategoryID=@catId";
                }
                else
                {
                    query = "SELECT * FROM Products";
                }

                SqlCommand cmd = new SqlCommand(query, conn);

                if (categoryId != null)
                {
                    cmd.Parameters.AddWithValue("@catId", categoryId);
                }

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Product p = new Product
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"]?.ToString(),
                        Description = reader["Description"]?.ToString(),
                        Price = (decimal)reader["Price"],
                        Stock = (int)reader["Stock"],
                        CategoryID = (int)reader["CategoryID"],
                        ImageUrl = reader["ImageUrl"]?.ToString() ?? ""
                    };

                    products.Add(p);
                }
            }

            return View(products);
        }

        // ÜRÜN DETAY
        public IActionResult Details(int id)
        {
            Product product = new Product();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Products WHERE ProductID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    product.ProductID = (int)reader["ProductID"];
                    product.ProductName = reader["ProductName"]?.ToString();
                    product.Description = reader["Description"]?.ToString();
                    product.Price = (decimal)reader["Price"];
                    product.Stock = (int)reader["Stock"];
                    product.CategoryID = (int)reader["CategoryID"];
                }
            }

            return View(product);
        }

        // SEPETE EKLE
        public IActionResult AddToCart(int id)
        {
            List<CartItem> cart;

            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData != null)
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
            }
            else
            {
                cart = new List<CartItem>();
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Products WHERE ProductID=@id";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    CartItem item = new CartItem
                    {
                        ProductID = (int)reader["ProductID"],
                        ProductName = reader["ProductName"]?.ToString(),
                        Price = (decimal)reader["Price"],
                        Quantity = 1
                    };

                    var existingItem = cart.FirstOrDefault(x => x.ProductID == id);

                    if (existingItem != null)
                    {
                        existingItem.Quantity++;
                    }
                    else
                    {
                        cart.Add(item);
                    }
                }
            }

            HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            HttpContext.Session.SetString("CartCount", cart.Count.ToString());
            TempData["Message"] = "Ürün sepete eklendi!";

            return RedirectToAction("Index");
        }
        public IActionResult Cart()
        {
            List<CartItem> cart;

            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData != null)
            {
                cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();
            }
            else
            {
                cart = new List<CartItem>();
            }

            return View(cart);
        }
        public IActionResult RemoveFromCart(int id)
        {
            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData != null)
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData);

                if (cart != null)
                {
                    var item = cart.FirstOrDefault(x => x.ProductID == id);
                    if (item != null)
                    {
                        cart.Remove(item);
                    }

                    HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
                }
            }

            return RedirectToAction("Cart");
        }
        public IActionResult IncreaseQuantity(int id)
        {
            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData != null)
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();

                var item = cart.FirstOrDefault(x => x.ProductID == id);
                if (item != null)
                {
                    item.Quantity++;
                }

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }

            return RedirectToAction("Cart");
        }
        public IActionResult DecreaseQuantity(int id)
        {
            var sessionData = HttpContext.Session.GetString("Cart");

            if (sessionData != null)
            {
                var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();

                var item = cart.FirstOrDefault(x => x.ProductID == id);
                if (item != null)
                {
                    item.Quantity--;

                    if (item.Quantity <= 0)
                    {
                        cart.Remove(item);
                    }
                }

                HttpContext.Session.SetString("Cart", JsonSerializer.Serialize(cart));
            }

            return RedirectToAction("Cart");
        }
        public IActionResult Checkout()
        {
            var sessionData = HttpContext.Session.GetString("Cart");
            var userEmail = HttpContext.Session.GetString("UserEmail");

            if (sessionData == null || userEmail == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var cart = JsonSerializer.Deserialize<List<CartItem>>(sessionData) ?? new List<CartItem>();

            decimal total = 0;
            foreach (var item in cart)
            {
                total += item.Price * item.Quantity;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "INSERT INTO Orders (UserID, TotalPrice) VALUES (@userId, @total)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@userId", 1); // şimdilik sabit
                cmd.Parameters.AddWithValue("@total", total);

                cmd.ExecuteNonQuery();
            }

            // Sepeti temizle
            HttpContext.Session.Remove("Cart");

            return View("OrderSuccess");
        }
        public IActionResult Orders()
        {
            List<Order> orders = new List<Order>();

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Orders WHERE UserID = 1 ORDER BY OrderDate DESC";
                SqlCommand cmd = new SqlCommand(query, conn);

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Order o = new Order
                    {
                        OrderID = (int)reader["OrderID"],
                        UserID = (int)reader["UserID"],
                        TotalPrice = (decimal)reader["TotalPrice"],
                        OrderDate = (DateTime)reader["OrderDate"]
                    };

                    orders.Add(o);
                }
            }

            return View(orders);
        }
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var user = HttpContext.Session.GetString("UserEmail");

            if (user == null)
            {
                context.Result = RedirectToAction("Login", "Account");
            }

            base.OnActionExecuting(context);
        }
    }
}