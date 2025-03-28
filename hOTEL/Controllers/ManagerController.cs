using Hotel.Models.Entities;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Hotel.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin,Manager")] // Allow only Managers and Admins
    public class ManagerController : ControllerBase
    {
        private readonly IManagerService _managerService;

        public ManagerController(IManagerService managerService)
        {
            _managerService = managerService;
        }

        // ✅ Register a Manager (Only Admins)
        [HttpPost("register")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> RegisterManager([FromBody] Manager manager, [FromQuery] string username, [FromQuery] string password)
        {
            try
            {
                await _managerService.AddManager(manager, username, password);
                return Ok("Manager registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update Manager Information (Only Managers & Admins)
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateManager(int id, [FromBody] Manager updatedManager)
        {
            try
            {
                bool result = await _managerService.UpdateManager(id, updatedManager);
                if (!result) return NotFound("Manager not found.");
                return Ok("Manager updated successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Assign Manager to Hotel (Only Admins)
        [HttpPost("{managerId}/assign-hotel/{hotelId}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> AssignManagerToHotel(int managerId, int hotelId)
        {
            try
            {
                await _managerService.AssignManagerToHotel(managerId, hotelId);
                return Ok("Manager assigned to hotel successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Remove Manager (Only Admins)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> RemoveManager(int id)
        {
            try
            {
                bool result = await _managerService.DeleteManager(id);
                if (!result) return NotFound("Manager not found.");
                return Ok("Manager removed successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Get All Managers (Only Admins)
        [HttpGet]
        [Authorize(Roles = "Admin")] 
        public async Task<IActionResult> GetAllManagers()
        {
            var managers = await _managerService.GetAllManagers();
            return Ok(managers);
        }

        // ✅ Get Manager by ID (Managers & Admins)
        [HttpGet("{id}")]
        public async Task<IActionResult> GetManagerById(int id)
        {
            var manager = await _managerService.GetManagerById(id);
            if (manager == null) return NotFound("Manager not found.");
            return Ok(manager);
        }
    }
}
