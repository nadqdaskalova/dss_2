namespace Todo.Web.Api.Models
{
    public class UpdateTaskInput
    {
        public string? Description { get; set; }
        public bool Completed { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
