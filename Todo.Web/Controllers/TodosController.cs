using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Todo.Web.Clients.Interfaces;
using Todo.Web.Models;

namespace Todo.Web.Controllers
{
    [Authorize]
    public class TodosController : Controller
    {
        private readonly ITodoListClient _todosClient;

        public TodosController(ITodoListClient client)
        {
            _todosClient = client;
        }

        public async Task<IActionResult> Index()
        {
            int userId = int.Parse(User.FindFirst("userId")!.Value!);
            var todosList = await _todosClient.GetAll(userId);

            var viewModels = todosList
                .Select(e => new TodoListViewModel { 
                    Id = e.Id, 
                    Description = e.Description, 
                    IsActive = e.IsActive, 
                    NumberOfTasks = e.NumberOfTasks})
                .ToArray();

            return View(viewModels);
        }
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description")] CreateTodoListViewModel todoListViewModel)
        {
            if (ModelState.IsValid)
            {
                int userId = int.Parse(User.FindFirst("userId")!.Value!);
                await _todosClient.CreateTodo(new Clients.Models.CreateTodoListInputModel
                {
                    Description = todoListViewModel.Description!
                }, userId);

                return RedirectToAction(nameof(Index));
            }

            return View(todoListViewModel);
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _todosClient.GetById(id);

            if (list == null)
            {
                return NotFound();
            }

            return RedirectToAction("Index", "Task", new { 
                todoListId = list.Id, 
                todoDescription = list.Description, 
                todoIsActive = list.IsActive, 
                todoNumOfTasks = list.NumberOfTasks });
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _todosClient.GetById(id);

            if (list == null)
            {
                return NotFound();
            }

            return View(new TodoListViewModel
            {
                Id = list.Id,
                Description = list.Description,
                IsActive = list.IsActive
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, [Bind("Id, Description, IsActive")] TodoListViewModel todoListModel)
        {
            if (id != todoListModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _todosClient.UpdateTodo(id, new Clients.Models.UpdateTodoListInputModel
                {
                    Description = todoListModel.Description,
                    IsActive = todoListModel.IsActive
                });

                return RedirectToAction(nameof(Index));
            }

            return View(todoListModel);
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var list = await _todosClient.GetById(id);

            if (list == null)
            {
                return NotFound();
            }

            return View(new TodoListViewModel
            {
                Id = list.Id,
                Description = list.Description,
                IsActive = list.IsActive,
                NumberOfTasks = list.NumberOfTasks
            });
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            await _todosClient.DeleteTodo(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
