using System;

namespace TodoWebApi.Models
{
    public class AddTodoModel
    {
        public string Description { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime Date { get; set; }
    }
}
