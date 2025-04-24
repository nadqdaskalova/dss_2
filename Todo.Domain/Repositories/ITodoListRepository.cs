using Todo.Domain.Models;

namespace Todo.Domain.Repositories
{
    public interface ITodoListRepository : IRepository<TodoList>
    {
        IEnumerable<TodoList> GetAll(int userID);
    }
}
