using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hotel.Models.Entities;

namespace Hotel.Service
{
    public interface IReservationService
    {
        Task<bool> CreateReservation(int guestId, int roomId, DateTime checkIn, DateTime checkOut);
        Task<bool> UpdateReservationDates(int reservationId, DateTime newCheckIn, DateTime newCheckOut);
        Task<bool> CancelReservation(int reservationId);
        Task<IEnumerable<Reservation>> SearchReservations(int? hotelId, int? guestId, int? roomId, DateTime? date, bool? isActive);
    }
}
