namespace Todo.Web.Models
{
    public class TodoListViewModel
    {
        public int? Id { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public int NumberOfTasks { get; set; }
    }
    public class CreateTodoListViewModel
    {
        public string? Description { get; set; }
    }
}
