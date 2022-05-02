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
        var todoListViewModel = GetAllToDos();
        return View(todoListViewModel);
    }

    internal ToDoViewModel GetAllToDos()
    {
        List<ToDoModel> todoList = new List<ToDoModel>();

        using(SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"SELECT * FROM ToDo";
            
            using(var reader = command.ExecuteReader())
            {
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                        todoList.Add(
                            new ToDoModel 
                            {
                                Id = reader.GetInt32(0),
                                Name = reader.GetString(1)
                            }
                        );
                    }
                }
                else 
                {
                    return new ToDoViewModel 
                    {
                        ToDoList = todoList
                    };
                }
            }


        }
        return new ToDoViewModel
        {
            ToDoList = todoList
        };
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
