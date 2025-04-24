namespace Todo.Web.Api.Models
{
    public class UpdateTodoListInput
    {
        public string? Description { get; set; }
        public bool IsActive { get; set; } = true;
    }
}
