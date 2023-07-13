namespace ToDo.Models
{
    public class TodoRepository
    {
        public static List<Todo> toDoItems { get; set; } = new List<Todo>()
        {
            new Todo
            {
                Id = 1,
                ToDoItems = "Create Passport"
            },
            new Todo
            {
                Id = 2,
                ToDoItems = "File ITR"
            },
            new Todo
            {
                Id = 3,
                ToDoItems = "Apply for credit card"
            },
            new Todo
            {
                Id = 4,
                ToDoItems = "Learn ASP.NET CORE WEB API"
            }
        };
    }
}
