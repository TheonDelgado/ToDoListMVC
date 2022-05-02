using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoListMVC.Models
{
    public class ToDoViewModel 
    {
        public List<ToDoModel> ToDoList { get; set; }
        public ToDoModel ToDo { get; set; }
    }
}