using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Models.Entities;
using Hotel.Service;

namespace HOTEL.API.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // ✅ Get all users (Only Admins)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _userService.GetAllUsers();
        }

        // ✅ Get a user by ID (Admins can get any user)
        [HttpGet("{id}")]
        [Authorize] 
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            var loggedInUserId = int.Parse(User.FindFirst("Id")?.Value);
            var isAdmin = User.IsInRole("Admin");

            if (isAdmin || loggedInUserId == id)
                return user;

            return Forbid(); 
        }

        // ✅ Create a new user (Publicly accessible for self-registration)
        [HttpPost]
        [AllowAnonymous] 
        public async Task<ActionResult<User>> CreateUser([FromBody] User user)
        {
            var createdUser = await _userService.CreateUser(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }

        // ✅ Delete user (Only Admins can delete any user, users can delete themselves)
        [HttpDelete("{id}")]
        [Authorize] 
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null) return NotFound("User not found.");

            var loggedInUserId = int.Parse(User.FindFirst("Id")?.Value);
            var isAdmin = User.IsInRole("Admin");

            if (isAdmin || loggedInUserId == id)
            {
                bool result = await _userService.DeleteUser(id);
                if (!result) return NotFound("User not found.");
                return NoContent();
            }

            return Forbid(); 
        }
    }
}
