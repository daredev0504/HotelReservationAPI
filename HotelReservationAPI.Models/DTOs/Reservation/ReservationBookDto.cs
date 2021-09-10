using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationAPI.Models.DTOs.Reservation
{
    public class ReservationBookDto
    {
        public List<string> RoomsToBeBooked { get; set; }
    }
}
