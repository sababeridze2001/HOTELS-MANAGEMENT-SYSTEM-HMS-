using Hotel.Models.Entities;
using Hotel.Repository;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Hotel.Service
{
    public class HotelService : IHotelService
    {
        private readonly HotelDbContext _context;

        public HotelService(HotelDbContext context)
        {
            _context = context;
        }

        
        public async Task<bool> AddHotel(Hotel.Models.Entities.Hotel hotel)
        {
            if (hotel.Rating < 1 || hotel.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            
            bool managerExists = await _context.Managers.AnyAsync(m => m.Id == hotel.ManagerId);
            if (!managerExists)
                throw new ArgumentException("A valid ManagerId must be provided.");

            
            bool hotelExists = await _context.Hotels
                .AnyAsync(h => h.Name == hotel.Name && h.Address == hotel.Address);

            if (hotelExists)
                throw new ArgumentException("A hotel with the same name and address already exists.");

            _context.Hotels.Add(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> UpdateHotel(int id, Hotel.Models.Entities.Hotel updatedHotel)
        {
            var hotel = await _context.Hotels.FindAsync(id);
            if (hotel == null)
                return false;

            if (updatedHotel.Rating < 1 || updatedHotel.Rating > 5)
                throw new ArgumentException("Rating must be between 1 and 5.");

            
            bool managerExists = await _context.Managers.AnyAsync(m => m.Id == updatedHotel.ManagerId);
            if (!managerExists)
                throw new ArgumentException("A valid ManagerId must be provided.");

            hotel.Name = updatedHotel.Name;
            hotel.Address = updatedHotel.Address;
            hotel.Rating = updatedHotel.Rating;
            hotel.ManagerId = updatedHotel.ManagerId; 

            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> DeleteHotel(int id)
        {
            var hotel = await _context.Hotels
                .Include(h => h.Rooms)
                .Include(h => h.Rooms.Select(r => r.Reservations))
                .FirstOrDefaultAsync(h => h.Id == id);

            if (hotel == null)
                return false;

            
            if (hotel.Rooms.Any() || hotel.Rooms.SelectMany(r => r.Reservations).Any())
                throw new InvalidOperationException("Cannot delete a hotel with active rooms or reservations.");

            _context.Hotels.Remove(hotel);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<IEnumerable<Hotel.Models.Entities.Hotel>> GetAllHotels(string? country, string? city, int? rating)
        {
            var query = _context.Hotels.AsQueryable();

            if (!string.IsNullOrEmpty(country))
                query = query.Where(h => h.Country == country);

            if (!string.IsNullOrEmpty(city))
                query = query.Where(h => h.City == city);

            if (rating.HasValue)
                query = query.Where(h => h.Rating == rating.Value);

            return await query.ToListAsync();
        }

        
        public async Task<Hotel.Models.Entities.Hotel?> GetHotelById(int id)
        {
            return await _context.Hotels.FindAsync(id);
        }
    }
}