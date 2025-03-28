using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Models.Entities;

namespace Hotel.Service
{
    public interface IGuestService
    {
        Task<IEnumerable<Guest>> GetAllGuests();
        Task<Guest?> GetGuestById(int id);
        Task<Guest> RegisterGuest(Guest guest); 
        Task<bool> UpdateGuest(int id, Guest updatedGuest);
        Task<bool> DeleteGuest(int id);
    }
}