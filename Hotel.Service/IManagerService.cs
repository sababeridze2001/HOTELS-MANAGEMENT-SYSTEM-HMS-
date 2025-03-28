using System.Collections.Generic;
using System.Threading.Tasks;
using Hotel.Models.Entities;

namespace Hotel.Service
{
    public interface IManagerService
    {
        Task<IEnumerable<Manager>> GetAllManagers();
        Task<Manager?> GetManagerById(int id);
        Task<bool> AddManager(Manager manager, string username, string password); 
        Task<bool> UpdateManager(int id, Manager updatedManager);
        Task<bool> AssignManagerToHotel(int managerId, int hotelId);
        Task<bool> DeleteManager(int id);
    }
}