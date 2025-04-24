using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Todo.Application.Services;
using Todo.Domain.Services;
using Todo.Web.Api.Models;

namespace Todo.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoTaskController : ControllerBase
    {
        private readonly ITodoTaskService _todoTaskService;
        public TodoTaskController(ITodoTaskService todoTaskService)
        {
            _todoTaskService = todoTaskService;
        }
        [HttpGet("GetById/{id}")]
        public IActionResult GetById([FromRoute, Required] int? id)
        {
            if (id == null)
            {
                return BadRequest("Something went wrong with getting the task");
            }

            var task = _todoTaskService.GetTodoTask(id.GetValueOrDefault());
            if (task == null)
            {
                return BadRequest("Something went wrong with getting the task");
            }

            return Ok(new { task.Id, task.Description, task.DueDate });
        }
        [HttpGet("GetAll")]
        public IActionResult GetAll(int listID)
        {
            var tasks = _todoTaskService.GetAll(listID);
            if (tasks == null)
            {
                return BadRequest("Something went wrong with getting the tasks");
            }

            return Ok(
                tasks
                .Select(task => new { task.Id, task.Description, task.DueDate, task.Completed })
                .ToArray());
        }
        [HttpPost("CreateTodoTask")]
        public IActionResult CreateTodoTask([FromBody] CreateTaskInput todoTask, int listID)
        {
            if (string.IsNullOrEmpty(todoTask.Description))
            {
                return BadRequest("Something went wrong with creating the task");
            }

            _todoTaskService.Create(listID, todoTask.Description, todoTask.DueDate);

            return Ok();
        }
        [HttpPut("UpdateTodoTask/{id}")]
        public IActionResult UpdateTask([FromRoute] int? id, [FromBody] UpdateTaskInput todoTask)
        {
            if (id is null
                || string.IsNullOrEmpty(todoTask.Description))
            {
                return BadRequest("Something went wrong with updating the task");
            }

            _todoTaskService.Update(
                id.GetValueOrDefault(),
                todoTask.Description,
                todoTask.Completed,
                todoTask.DueDate);

            return Ok();
        }
        [HttpDelete("DeleteTodoTask/{id}")]
        public IActionResult DeleteTask([FromRoute] int? id)
        {
            if (id is null)
            {
                return BadRequest("Something went wrong with deleting the task");
            }

            _todoTaskService.Delete(id.GetValueOrDefault());

            return Ok();
        }
    }
}
