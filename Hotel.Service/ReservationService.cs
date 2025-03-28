using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Models.Entities;
using Hotel.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service
{
    public class ReservationService : IReservationService
    {
        private readonly HotelDbContext _context;

        public ReservationService(HotelDbContext context)
        {
            _context = context;
        }

        
        public async Task<bool> CreateReservation(int guestId, int roomId, DateTime checkIn, DateTime checkOut)
        {
            
            if (checkIn < DateTime.Today || checkIn > DateTime.Today.AddDays(1))
                throw new ArgumentException("Check-in date must be today or tomorrow.");
            if (checkOut <= checkIn)
                throw new ArgumentException("Check-out date must be later than check-in date.");

            
            bool isRoomAvailable = await _context.Reservations
                .AnyAsync(r => r.RoomId == roomId && !(r.CheckOutDate <= checkIn || r.CheckInDate >= checkOut));

            if (isRoomAvailable)
                throw new InvalidOperationException("Room is not available for the selected dates.");

            
            var reservation = new Reservation
            {
                CheckInDate = checkIn,
                CheckOutDate = checkOut,
                RoomId = roomId,
                GuestReservations = new List<GuestReservation> { new GuestReservation { GuestId = guestId } }
            };

            _context.Reservations.Add(reservation);
            await _context.SaveChangesAsync();

            
            var room = await _context.Rooms.FindAsync(roomId);
            if (room != null)
            {
                room.IsOccupied = true;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        
        public async Task<bool> UpdateReservationDates(int reservationId, DateTime newCheckIn, DateTime newCheckOut)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null) return false;

            
            if (newCheckIn < DateTime.Today || newCheckOut <= newCheckIn)
                throw new ArgumentException("Invalid date selection.");

            
            bool isOverlapping = await _context.Reservations
                .AnyAsync(r => r.RoomId == reservation.RoomId && r.Id != reservationId &&
                              !(r.CheckOutDate <= newCheckIn || r.CheckInDate >= newCheckOut));

            if (isOverlapping)
                throw new InvalidOperationException("New dates overlap with another reservation.");

            
            reservation.CheckInDate = newCheckIn;
            reservation.CheckOutDate = newCheckOut;
            await _context.SaveChangesAsync();

            return true;
        }

       
        public async Task<bool> CancelReservation(int reservationId)
        {
            var reservation = await _context.Reservations.FindAsync(reservationId);
            if (reservation == null) return false;

            _context.Reservations.Remove(reservation);
            await _context.SaveChangesAsync();

            
            var room = await _context.Rooms.FindAsync(reservation.RoomId);
            if (room != null)
            {
                room.IsOccupied = false;
                await _context.SaveChangesAsync();
            }

            return true;
        }

        
        public async Task<IEnumerable<Reservation>> SearchReservations(int? hotelId, int? guestId, int? roomId, DateTime? date, bool? isActive)
        {
            var query = _context.Reservations.AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(r => r.Room.HotelId == hotelId.Value);
            if (guestId.HasValue)
                query = query.Where(r => r.GuestReservations.Any(gr => gr.GuestId == guestId.Value));
            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);
            if (date.HasValue)
                query = query.Where(r => r.CheckInDate <= date.Value && r.CheckOutDate >= date.Value);
            if (isActive.HasValue)
                query = query.Where(r => isActive.Value ? r.CheckOutDate >= DateTime.Today : r.CheckOutDate < DateTime.Today);

            return await query.ToListAsync();
        }
    }
}
