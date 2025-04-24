namespace Todo.Web.Clients.Models
{
    public class TodoTaskModel
    {
        public int Id { get; set; }
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime DueDate { get; set; }
    }
}
