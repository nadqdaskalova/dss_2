using Todo.Domain.Models;

namespace Todo.Domain.Repositories
{
    public interface ITodoTaskRepository : IRepository<TodoTask>
    {
        public IEnumerable<TodoTask> GetAll(int listID);

    }
}
