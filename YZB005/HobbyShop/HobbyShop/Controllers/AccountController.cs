using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace HobbyShop.Controllers
{
    public class AccountController : Controller
    {
        private string connectionString = "Server=YAĞMUR\\SQLEXPRESS;Database=HobbyShopDB;Trusted_Connection=True;TrustServerCertificate=True;";

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string query = "SELECT * FROM Users WHERE Email=@Email AND Password=@Password";

                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Email", email);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();

                if (reader.Read())
                {
                    HttpContext.Session.SetString("UserEmail", email);
                    HttpContext.Session.SetString("IsAdmin", "true");
                    return RedirectToAction("Index", "Dashboard");
                }
                else
                {
                    return Content("Email veya şifre yanlış!");
                }
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Register(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // 1. Aynı email var mı kontrol et
                string checkQuery = "SELECT COUNT(*) FROM Users WHERE Email=@email";
                SqlCommand checkCmd = new SqlCommand(checkQuery, conn);
                checkCmd.Parameters.AddWithValue("@email", email);

                int count = (int)checkCmd.ExecuteScalar();

                if (count > 0)
                {
                    ViewBag.Error = "Bu email zaten kayıtlı!";
                    return View();
                }

                // 2. Kayıt ekle
                string query = "INSERT INTO Users (Email, Password) VALUES (@email, @password)";
                SqlCommand cmd = new SqlCommand(query, conn);

                cmd.Parameters.AddWithValue("@email", email);
                cmd.Parameters.AddWithValue("@password", password);

                cmd.ExecuteNonQuery();
            }

            return RedirectToAction("Login");
        }
    }
}