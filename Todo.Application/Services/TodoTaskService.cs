using Todo.Domain.Models;
using Todo.Domain.Repositories;
using Todo.Domain.Services;

namespace Todo.Application.Services
{
    public class TodoTaskService : ITodoTaskService
    {
        private readonly ITodoListRepository _todoListRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITodoTaskRepository _repository;

        public TodoTaskService(
            ITodoListRepository todoListRepository,
            IUserRepository userRepository,
            ITodoTaskRepository repository)
        {
            _todoListRepository = todoListRepository;
            _userRepository = userRepository;
            _repository = repository;
        }

        public void Create(int? holderId, string description, DateTime dueDate)
        {
            var todoList = _todoListRepository.GetById(
                holderId.GetValueOrDefault());

            if (todoList is null)
            {
                throw new InvalidProgramException(
                    "Something went wrong. TodoList not found");
            }

            _repository.Create(new TodoTask
            {
                Description = description,
                DueDate = dueDate,
                Holder = todoList,
                Completed = false,
                TodoId = holderId
            });
        }

        public void Delete(int id)
        {
            var todoList = _repository.GetById(id);
            if (todoList is null)
            {
                throw new InvalidProgramException(
                    "Something went wrong. TodoList not found");
            }

            _repository.Delete(todoList);
        }

        public TodoTask? GetTodoTask(int id)
        {
            var todoTask = _repository.GetById(id);
            return todoTask;
        }

        public IEnumerable<TodoTask> GetAll(int listID)
        {
            var todoTask = _repository.GetAll(listID);
            return todoTask;
        }

        public void Update(
            int id,
            string? description,
            bool Completed,
            DateTime? dueDate)
        {
            var todoTask = _repository.GetById(id);
            if (todoTask is null)
            {
                throw new InvalidProgramException(
                    "Something went wrong. TodoList not found");
            }

            if(description is not null)
                todoTask.Description = description;
            if (dueDate is not null)
                todoTask.DueDate = (DateTime)dueDate;

            todoTask.Completed = Completed;

            _repository.Update(todoTask);
        }
    }
}
