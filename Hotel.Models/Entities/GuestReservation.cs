using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel.Models.Entities
{
    public class GuestReservation
    {

        public int GuestId { get; set; }
        [ForeignKey("GuestId")]
        public Guest Guest { get; set; }

        public int ReservationId { get; set; }
        [ForeignKey("ReservationId")]
        public Reservation Reservation { get; set; }
    }
}
