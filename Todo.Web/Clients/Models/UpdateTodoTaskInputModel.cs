namespace Todo.Web.Clients.Models
{
    public class UpdateTodoTaskInputModel
    {
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
