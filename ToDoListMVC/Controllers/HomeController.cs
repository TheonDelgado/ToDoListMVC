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

    [HttpPost]
    public JsonResult Delete(int id) 
    {
        using(SqliteConnection connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
        {
            connection.Open();
            var command = connection.CreateCommand();
            command.CommandText = @$"DELETE from ToDo
                                    WHERE Id = '{id}';";
            command.ExecuteNonQuery();
        }
        return Json(new{});
    }

    public JsonResult PopulateForm(int id) 
    {
        var todo = GetById(id);
        return Json(todo);
    }

     internal ToDoModel GetById(int id)
        {
            ToDoModel todo = new();

            using (var connection = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"SELECT * FROM todo Where Id = '{id}'";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            todo.Id = reader.GetInt32(0);
                            todo.Name = reader.GetString(1);
                        }
                        else
                        {
                            return todo;   
                        }
                    };
                }
            }

            return todo;
        }

     public RedirectResult Update(ToDoModel todo)
        {
            using (SqliteConnection con = new SqliteConnection(_configuration.GetConnectionString("ConnectionString")))
            {
                using (var tableCmd = con.CreateCommand())
                {
                    con.Open();
                    tableCmd.CommandText = $"UPDATE todo SET name = '{todo.Name}' WHERE Id = '{todo.Id}';";
                    try
                    {
                        tableCmd.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }

            return Redirect("https://localhost:7017/");
        }

        public void ResetIndex(ToDoModel todo) 
        {
            
        }
}
