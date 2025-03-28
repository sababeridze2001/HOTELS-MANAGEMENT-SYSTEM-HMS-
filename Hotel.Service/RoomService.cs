
using System.Text;
using Hotel.Models.Entities;
using Hotel.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;



namespace Hotel.Service
{


    public class RoomService : IRoomService
    {
        private readonly HotelDbContext _context;

        public RoomService(HotelDbContext context)
        {
            _context = context;
        }

       

        public async Task<int?> AddRoom(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
            return room.Id; 
        }


        
        public async Task<bool> UpdateRoom(int roomId, Room updatedRoom)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            if (updatedRoom.Price <= 0)
                throw new ArgumentException("Price must be greater than zero.");

            room.Name = updatedRoom.Name;
            room.IsAvailable = updatedRoom.IsAvailable;
            room.Price = updatedRoom.Price;

            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> SetRoomAvailability(int roomId, bool isAvailable)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            if (room == null) return false;

            room.IsAvailable = isAvailable;
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> DeleteRoom(int roomId)
        {
            var room = await _context.Rooms.Include(r => r.Reservations).FirstOrDefaultAsync(r => r.Id == roomId);
            if (room == null) return false;

            if (room.Reservations != null && room.Reservations.Any())
                throw new InvalidOperationException("Room cannot be deleted as it has active bookings.");

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<IEnumerable<Room>> GetRooms(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice)
        {
            var query = _context.Rooms.AsQueryable();

            if (hotelId.HasValue)
                query = query.Where(r => r.HotelId == hotelId);
            if (isAvailable.HasValue)
                query = query.Where(r => r.IsAvailable == isAvailable);
            if (minPrice.HasValue)
                query = query.Where(r => r.Price >= minPrice);
            if (maxPrice.HasValue)
                query = query.Where(r => r.Price <= maxPrice);

            return await query.ToListAsync();


        }

        public async Task<Room?> GetRoomById(int id)
        {
            try
            {
                var room = await _context.Rooms.Include(r => r.Hotel).FirstOrDefaultAsync(r => r.Id == id);

                if (room == null)
                {
                    Console.WriteLine($"Room with ID {id} not found.");
                }

                return room;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetRoomById: {ex.Message}"); 
                throw;
            }
        }
    }
}