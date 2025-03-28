using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Models.Entities;
using Hotel.Repository;
using Microsoft.EntityFrameworkCore;

namespace Hotel.Service
{
    public class ManagerService : IManagerService
    {
        private readonly HotelDbContext _context;

        public ManagerService(HotelDbContext context)
        {
            _context = context;
        }

        
        public async Task<bool> AddManager(Manager manager, string username, string password)
        {
            if (await _context.Managers.AnyAsync(m => m.Email == manager.Email))
                throw new ArgumentException("A manager with this email already exists.");

            if (await _context.Managers.AnyAsync(m => m.PersonalNumber == manager.PersonalNumber))
                throw new ArgumentException("A manager with this personal number already exists.");

            
            var user = new User
            {
                Username = username,
                Password = password, 
                Role = "Manager"
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            
            manager.UserId = user.Id;
            _context.Managers.Add(manager);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> UpdateManager(int id, Manager updatedManager)
        {
            var manager = await _context.Managers.FindAsync(id);
            if (manager == null)
                return false;

            manager.Name = updatedManager.Name;
            manager.Surname = updatedManager.Surname;
            manager.Email = updatedManager.Email;
            manager.MobileNumber = updatedManager.MobileNumber;

            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> AssignManagerToHotel(int managerId, int hotelId)
        {
            var hotel = await _context.Hotels.FindAsync(hotelId);
            var manager = await _context.Managers.FindAsync(managerId);

            if (hotel == null || manager == null)
                return false;

            hotel.ManagerId = managerId;
            hotel.Manager = manager;

            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<bool> DeleteManager(int managerId)
        {
            var manager = await _context.Managers.FindAsync(managerId);
            if (manager == null)
                return false;

            var hotel = await _context.Hotels.FirstOrDefaultAsync(h => h.ManagerId == managerId);
            if (hotel == null)
                return false;

            
            var anotherManagerExists = await _context.Managers.AnyAsync(m => m.Id != managerId);
            if (!anotherManagerExists)
                throw new InvalidOperationException("Cannot remove the only manager of a hotel.");

            _context.Managers.Remove(manager);
            await _context.SaveChangesAsync();
            return true;
        }

        
        public async Task<IEnumerable<Manager>> GetAllManagers()
        {
            return await _context.Managers.ToListAsync();
        }

        
        public async Task<Manager?> GetManagerById(int id)
        {
            return await _context.Managers.FindAsync(id);
        }
    }
}
