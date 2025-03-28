using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Models.Entities;

namespace Hotel.Service
{
    public interface IRoomService
    {

        Task<int?> AddRoom(Room room);
        Task<bool> UpdateRoom(int id, Room updatedRoom);
        Task<bool> DeleteRoom(int id);
        Task<Room> GetRoomById(int id);
        Task<IEnumerable<Room>> GetRooms(int? hotelId, bool? isAvailable, decimal? minPrice, decimal? maxPrice);

        Task<bool> SetRoomAvailability(int roomId, bool isAvailable);

    }
}
