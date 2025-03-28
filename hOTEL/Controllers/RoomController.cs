using Hotel.Models.Entities;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HOTEL.API.Controllers
{
    [Route("api/rooms")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }

        // ✅ Add a new room to a hotel (Only Managers & Admins)
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> AddRoom([FromBody] Room room)
        {
            try
            {
                var roomId = await _roomService.AddRoom(room);
                if (roomId.HasValue)
                    return CreatedAtAction(nameof(GetRoomById), new { id = roomId.Value }, room);

                return BadRequest("Failed to add room.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update room details (Only Managers & Admins)
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateRoom(int id, [FromBody] Room updatedRoom)
        {
            try
            {
                var result = await _roomService.UpdateRoom(id, updatedRoom);
                if (result)
                    return Ok("Room updated successfully.");

                return NotFound("Room not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Manage room availability (Only Managers & Admins)
        [HttpPatch("{id}/availability")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> SetRoomAvailability(int id, [FromBody] bool isAvailable)
        {
            var result = await _roomService.SetRoomAvailability(id, isAvailable);
            if (result)
                return Ok("Room availability updated.");

            return NotFound("Room not found.");
        }

        // ✅ Delete a room (Only Managers & Admins, only if no active bookings)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            try
            {
                var result = await _roomService.DeleteRoom(id);
                if (result)
                    return Ok("Room deleted successfully.");

                return NotFound("Room not found or has active bookings.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Get all rooms with optional filters (Open to all authenticated users)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetRooms([FromQuery] int? hotelId, [FromQuery] bool? isAvailable, [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            var rooms = await _roomService.GetRooms(hotelId, isAvailable, minPrice, maxPrice);
            return Ok(rooms);
        }

        // ✅ Get a room by ID (Open to all authenticated users)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRoomById(int id)
        {
            var room = await _roomService.GetRoomById(id);
            if (room == null)
                return NotFound("Room not found.");

            return Ok(room);
        }
    }
}
