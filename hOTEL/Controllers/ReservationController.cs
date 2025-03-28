using Hotel.Models.Entities;
using Hotel.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace HOTEL.API.Controllers
{
    [Route("api/reservations")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        // ✅ Create Reservation (Only Users can create their own reservation)
        [HttpPost]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CreateReservation(int guestId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            try
            {
                await _reservationService.CreateReservation(guestId, roomId, checkIn, checkOut);
                return Ok("Reservation created successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Update Reservation (Only Users can update their own reservations)
        [HttpPut("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> UpdateReservation(int id, DateTime newCheckIn, DateTime newCheckOut)
        {
            try
            {
                bool result = await _reservationService.UpdateReservationDates(id, newCheckIn, newCheckOut);
                if (!result) return NotFound("Reservation not found.");
                return Ok("Reservation updated successfully.");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ✅ Cancel Reservation (Only Users can cancel their own reservations)
        [HttpDelete("{id}")]
        [Authorize(Roles = "User")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            bool result = await _reservationService.CancelReservation(id);
            if (!result) return NotFound("Reservation not found.");
            return Ok("Reservation canceled successfully.");
        }

        // ✅ Admin-Only: Search Reservations (Admins can view all reservations)
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> SearchReservations(int? hotelId, int? guestId, int? roomId, DateTime? date, bool? isActive)
        {
            var reservations = await _reservationService.SearchReservations(hotelId, guestId, roomId, date, isActive);
            return Ok(reservations);
        }
    }
}
