using Hotel.Models.Entities;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace HOTEL.API.Controllers
{
    [Route("api/guests")]
    [ApiController]
    public class GuestController : ControllerBase
    {
        private readonly IGuestService _guestService;

        public GuestController(IGuestService guestService)
        {
            _guestService = guestService;
        }

        // ✅ Open to everyone (Guest Registration)
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterGuest([FromBody] Guest guest)
        {
            try
            {
                await _guestService.RegisterGuest(guest);
                return Ok("Guest registered successfully.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Only guests themselves can update their information
        [HttpPut("{id}")]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> UpdateGuest(int id, [FromBody] Guest updatedGuest)
        {
            bool result = await _guestService.UpdateGuest(id, updatedGuest);
            if (!result) return NotFound("Guest not found.");
            return Ok("Guest updated successfully.");
        }

        // ✅ Only guests themselves can delete their account
        [HttpDelete("{id}")]
        [Authorize(Roles = "Guest")]
        public async Task<IActionResult> DeleteGuest(int id)
        {
            try
            {
                bool result = await _guestService.DeleteGuest(id);
                if (!result) return NotFound("Guest not found.");
                return Ok("Guest deleted successfully.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Only Admins can see all guests
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllGuests()
        {
            var guests = await _guestService.GetAllGuests();
            return Ok(guests);
        }

        // ✅ Guests can see their own profile, Admins can see all guests
        [HttpGet("{id}")]
        [Authorize(Roles = "Guest,Admin")]
        public async Task<IActionResult> GetGuestById(int id)
        {
            var guest = await _guestService.GetGuestById(id);
            if (guest == null) return NotFound("Guest not found.");
            return Ok(guest);
        }
    }
}
