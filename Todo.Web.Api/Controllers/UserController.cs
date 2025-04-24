using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Todo.Domain.Services;
using Todo.Web.Api.Models;

namespace Todo.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet("GetById/{id}")]
        public IActionResult GetById([FromRoute, Required] int? id)
        {
            if (id == null)
            {
                return BadRequest("Something went wrong with getting the user");
            }

            var user = _userService.GetUser(id.Value);
            if (user == null)
            {
                return BadRequest("Something went wrong with getting the user");
            }

            return Ok(new { user.Id, user.Name, user.IsAdmin });
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var users = _userService.GetUsers();
            if (users == null)
            {
                return BadRequest("Something went wrong with getting the users");
            }

            return Ok(users.Select(user => new { user.Id, user.Name, user.IsAdmin }).ToArray());
        }

        [HttpPost("CreateUser")]
        public IActionResult CreateUser([FromBody] CreateUserInput user)
        {
            if (string.IsNullOrEmpty(user.Password) || string.IsNullOrEmpty(user.Name))
                return BadRequest("Name and password are required.");

            var userId = _userService.Create(user.Name, user.Password, user.IsAdmin);

            return Ok(userId);
        }

        [HttpPut("UpdateUser/{id}")]
        public IActionResult UpdateUser([FromRoute] int? id, [FromBody] UpdateUserInput user)
        {
            if (id is null || string.IsNullOrEmpty(user.Name))
                return BadRequest("Valid user ID and name are required.");

            _userService.Update(id.Value, user.Name);

            return Ok();
        }

        [HttpDelete("DeleteUser/{id}")]
        public IActionResult DeleteUser([FromRoute] int? id)
        {
            if (id is null)
            {
                return BadRequest("Something went wrong with deleting the user");
            }

            _userService.Delete(id.Value);

            return Ok();
        }

        [HttpPost("ValidatePassword")]
        public ActionResult<int> ValidatePassword([FromBody] ValidatePasswordInput validatePassword)
        {
            if (string.IsNullOrEmpty(validatePassword.Password) || string.IsNullOrEmpty(validatePassword.Name))
                return BadRequest("Username and password are required.");

            var result = _userService.ValidatePassword(validatePassword.Name, validatePassword.Password);
            if (result <= 0)
                return Unauthorized();

            return Ok(result);
        }
    }
}
