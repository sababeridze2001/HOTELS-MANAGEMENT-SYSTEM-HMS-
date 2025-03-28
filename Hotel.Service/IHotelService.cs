using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Service
{
    public interface IHotelService
    {

        Task<bool> AddHotel(Hotel.Models.Entities.Hotel hotel);
        Task<bool> UpdateHotel(int id, Hotel.Models.Entities.Hotel updatedHotel);
        Task<bool> DeleteHotel(int id);
        Task<IEnumerable<Hotel.Models.Entities.Hotel>> GetAllHotels(string? country, string? city, int? rating);
        Task<Hotel.Models.Entities.Hotel?> GetHotelById(int id);
    }
}
