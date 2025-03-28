using Hotel.Models.Entities;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HOTEL.API.Controllers
{
    [Route("api/hotels")]
    [ApiController]
    public class HotelController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // ✅ Create a new hotel (Only Managers and Admins)
        [HttpPost]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> CreateHotel([FromBody] Hotel.Models.Entities.Hotel hotel)
        {
            try
            {
                var result = await _hotelService.AddHotel(hotel);
                if (result)
                    return CreatedAtAction(nameof(GetHotelById), new { id = hotel.Id }, hotel);

                return BadRequest("Failed to create hotel.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update hotel details (Only Managers and Admins)
        [HttpPut("{id}")]
        [Authorize(Roles = "Manager,Admin")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] Hotel.Models.Entities.Hotel updatedHotel)
        {
            try
            {
                var result = await _hotelService.UpdateHotel(id, updatedHotel);
                if (result)
                    return Ok("Hotel updated successfully.");

                return NotFound("Hotel not found.");
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Delete a hotel (Only Admins)
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var result = await _hotelService.DeleteHotel(id);
                if (result)
                    return Ok("Hotel deleted successfully.");

                return NotFound("Hotel not found.");
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Get all hotels (Accessible to all users)
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllHotels([FromQuery] string? country, [FromQuery] string? city, [FromQuery] int? rating)
        {
            var hotels = await _hotelService.GetAllHotels(country, city, rating);
            return Ok(hotels);
        }

        // ✅ Get a hotel by ID (Accessible to all users)
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetHotelById(int id)
        {
            var hotel = await _hotelService.GetHotelById(id);
            if (hotel == null)
                return NotFound("Hotel not found.");

            return Ok(hotel);
        }
    }
}
