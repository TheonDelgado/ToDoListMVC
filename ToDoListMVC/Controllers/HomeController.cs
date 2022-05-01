using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ToDoListMVC.Models;
using Microsoft.Data.Sqlite;

namespace ToDoListMVC.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;
   

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public IActionResult Index()
    {
        return View();
    }

    public RedirectResult Insert(ToDoModel todo)
    {
        using (SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = $@"INSERT INTO ToDo (Name)
                                    VALUES ('{todo.Name}');";
            command.ExecuteNonQuery();
        }
        return Redirect("https://localhost:7017");
    }
}
