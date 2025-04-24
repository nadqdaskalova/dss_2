using Refit;
using System.ComponentModel.DataAnnotations;
using Todo.Web.Clients.Models;

namespace Todo.Web.Clients.Interfaces
{
    public interface ITodoTaskClient
    {
        [Get("/GetById/{id}")]
        Task<TodoTaskModel?> GetById([Required] int? id);
        [Get("/GetAll")]
        Task<TodoTaskModel[]> GetAll(int listID);
        [Post("/CreateTodoTask")]
        Task CreateTask([Body] CreateTodoTaskInputModel task, int listID);
        [Put("/UpdateTodoTask/{id}")]
        Task UpdateTask([Required] int? id, [Body] UpdateTodoTaskInputModel task);
        [Delete("/DeleteTodoTask/{id}")]
        Task DeleteTask([Required] int? id);
    }
}
