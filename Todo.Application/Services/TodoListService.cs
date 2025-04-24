using Todo.Domain.Models;
using Todo.Domain.Repositories;
using Todo.Domain.Services;

namespace Todo.Application.Services
{
    public class TodoListService : ITodoListService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITodoListRepository _repository;

        public TodoListService(
            IUserRepository userRepository,
            ITodoListRepository repository)
        {
            _userRepository = userRepository;
            _repository = repository;
        }

        public void Create(int? ownerId, string description)
        {
            var existingUser = _userRepository.GetById(
                ownerId.GetValueOrDefault());

            if (existingUser is null)
            {
                throw new InvalidProgramException(
                    "Something went wrong. User not found");
            }

            _repository.Create(new TodoList
            {
                Date = DateTime.Now,
                Description = description,
                UserId = ownerId,
                IsActive = true,
                Owner = existingUser
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

        public TodoList? GetTodoList(int id)
        {
            var todoList = _repository.GetById(id);
            return todoList;
        }

        public IEnumerable<TodoList> GetTodoLists(int userID)
        {
            var todoList = _repository.GetAll(userID);
            return todoList;
        }

        public void Update(
            int id,
            string? description,
            bool isActive,
            int? numberOfTasks)
        {
            var todoList = _repository.GetById(id);
            if (todoList is null)
            {
                throw new InvalidProgramException(
                    "Something went wrong. TodoList not found");
            }

            if(!string.IsNullOrEmpty(description))
                todoList.Description = description;

            if (numberOfTasks != null)
                todoList.NumberOfTasks = (int)numberOfTasks;
            todoList.IsActive = isActive;

            _repository.Update(todoList);
        }
    }
}
