using Humanizer.Localisation.TimeToClockNotation;
using Refit;
using System.ComponentModel.DataAnnotations;
using Todo.Web.Clients.Models;

namespace Todo.Web.Clients.Interfaces
{
    public interface ITodoListClient
    {
        [Get("/GetById/{id}")]
        Task<TodoListModel?> GetById([Required] int? id);
        [Get("/GetAll")]
        Task<TodoListModel[]> GetAll(int userID);
        [Post("/CreateTodoList")]
        Task CreateTodo([Body] CreateTodoListInputModel todo, int userID);
        [Put("/UpdateTodoList/{id}")]
        Task UpdateTodo([Required] int? id, [Body] UpdateTodoListInputModel todo);
        [Delete("/DeleteTodoList/{id}")]
        Task DeleteTodo([Required] int? id);
    }
}
