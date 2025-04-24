using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todo.Web.Clients.Interfaces;
using Todo.Web.Clients.Models;
using Todo.Web.Models;

namespace Todo.Web.Controllers
{
    public class TaskController : Controller
    {
        private readonly ITodoTaskClient _taskClient;

        public TaskController(ITodoTaskClient client)
        {
            _taskClient = client;
        }

        // GET: Task
        public async Task<IActionResult> Index(
            int todoListId, 
            string todoDescription, 
            bool todoIsActive)
        {
            var tasks = await _taskClient.GetAll(todoListId);

            ViewBag.ListDescription = todoDescription;
            ViewBag.ListIsActive = todoIsActive;
            ViewBag.ListID = todoListId;

            TempData["ListId"] = todoListId;
            TempData["ListDescription"] = todoDescription;
            TempData["ListIsActive"] = todoIsActive;

            var viewModels = tasks
                .Select(e => new TodoTaskViewModel {
                    Id = e.Id, 
                    Description = e.Description, 
                    Completed = e.Completed, 
                    DueDate = e.DueDate })
                .ToArray();

            return View(viewModels);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description, DueDate, id")] CreateTodoTaskViewModel taskViewModel, int? id)
        {
            if (ModelState.IsValid)
            {
                int listId = Convert.ToInt32(TempData["ListId"]);
                string desc = TempData["ListDescription"]?.ToString();
                bool isActive = Convert.ToBoolean(TempData["ListIsActive"]);


                if (id == null)
                {
                    return NotFound();
                }

                await _taskClient.CreateTask(new Clients.Models.CreateTodoTaskInputModel
                {
                    Description = taskViewModel.Description!,
                    DueDate = taskViewModel.DueDate
                }, (int)id!);

                return RedirectToAction("Index", new
                {
                    todoListId = listId,
                    todoDescription = desc,
                    todoIsActive = isActive
                });
            }
             
            return View(taskViewModel);
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskClient.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(new TodoTaskViewModel
            {
                Id = task.Id,
                Description = task.Description,
                Completed = task.Completed,
                DueDate = task.DueDate
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Description, Completed, DueDate")] TodoTaskViewModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                int listId = Convert.ToInt32(TempData["ListId"]);
                string desc = TempData["ListDescription"]?.ToString();
                bool isActive = Convert.ToBoolean(TempData["ListIsActive"]);

                await _taskClient.UpdateTask(id, new Clients.Models.UpdateTodoTaskInputModel
                {
                    Description = taskModel.Description,
                    Completed = taskModel.Completed,
                    DueDate = taskModel.DueDate
                });

                return RedirectToAction("Index", new
                {
                    todoListId = listId,
                    todoDescription = desc,
                    todoIsActive = isActive
                });
            }

            return View(taskModel);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var task = await _taskClient.GetById(id);

            if (task == null)
            {
                return NotFound();
            }

            return View(new TodoTaskViewModel
            {
                Id = task.Id,
                Description = task.Description,
                Completed = task.Completed,
                DueDate = task.DueDate
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            int listId = Convert.ToInt32(TempData["ListId"]);
            string desc = TempData["ListDescription"]?.ToString();
            bool isActive = Convert.ToBoolean(TempData["ListIsActive"]);

            await _taskClient.DeleteTask(id);

            return RedirectToAction("Index", new
            {
                todoListId = listId,
                todoDescription = desc,
                todoIsActive = isActive,
            });
        }
    }
}
