using System;

namespace TodoWebApp.Model
{
    public class ToDo
    {
        public int ToDoId { get; set; }
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; } = DateTime.Today;

    }
}
