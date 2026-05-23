using HobbyShop.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;

public class DashboardController : Controller
{
    string connectionString = "Server=YAĞMUR\\SQLEXPRESS;Database=HobbyShopDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public IActionResult Index()
    {
        List<Product> products = new List<Product>();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();

            string query = "SELECT TOP 4 * FROM Products"; // popüler ürünler
            SqlCommand cmd = new SqlCommand(query, conn);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                products.Add(new Product
                {
                    ProductID = (int)reader["ProductID"],
                    ProductName = reader["ProductName"]?.ToString() ?? "",
                    Price = (decimal)reader["Price"]
                });
            }
        }

        return View(products);
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
