using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hotel.Models.Entities;

namespace Hotel.Service
{
    public class GuestService : IGuestService
    {
        private readonly List<Guest> _guests = new(); 
        private int _nextId = 1; 

        
        public async Task<Guest> RegisterGuest(Guest guest)
        {
            if (_guests.Any(g => g.PersonalNumber == guest.PersonalNumber))
                throw new ArgumentException("A guest with this personal number already exists.");

            if (_guests.Any(g => g.MobileNumber == guest.MobileNumber))
                throw new ArgumentException("A guest with this mobile number already exists.");

            guest.Id = _nextId++; 
            _guests.Add(guest);
            return await Task.FromResult(guest); 
        }

        
        public async Task<bool> UpdateGuest(int id, Guest updatedGuest)
        {
            var guest = _guests.FirstOrDefault(g => g.Id == id);
            if (guest == null)
                return await Task.FromResult(false);

            guest.Name = updatedGuest.Name ?? guest.Name;
            guest.Surname = updatedGuest.Surname ?? guest.Surname;
            guest.MobileNumber = updatedGuest.MobileNumber ?? guest.MobileNumber;

            return await Task.FromResult(true);
        }

        
        public async Task<bool> DeleteGuest(int id)
        {
            var guest = _guests.FirstOrDefault(g => g.Id == id);
            if (guest == null)
                return await Task.FromResult(false);

            if (guest.GuestReservations?.Any() == true)
                throw new InvalidOperationException("Cannot delete a guest with active reservations.");

            _guests.Remove(guest);
            return await Task.FromResult(true);
        }

        
        public async Task<IEnumerable<Guest>> GetAllGuests()
        {
            return await Task.FromResult(_guests);
        }

        
        public async Task<Guest?> GetGuestById(int id)
        {
            return await Task.FromResult(_guests.FirstOrDefault(g => g.Id == id));
        }
    }
}
