namespace Todo.Web.Api.Models
{
    public class CreateTaskInput
    {
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
