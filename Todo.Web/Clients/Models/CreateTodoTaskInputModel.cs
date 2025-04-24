using System.ComponentModel.DataAnnotations;

namespace Todo.Web.Clients.Models
{
    public class CreateTodoTaskInputModel
    {
        [Required]
        public string Description { get; set; } = string.Empty;
        [Required]
        public DateTime DueDate { get; set; }
    }
}
