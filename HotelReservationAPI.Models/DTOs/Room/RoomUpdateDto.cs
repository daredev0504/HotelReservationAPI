using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationAPI.Models.DTOs.Room
{
    public class RoomUpdateDto
    {
        public decimal Price { get; set; }
        public string RoomType { get; set; }
        public bool IsBooked { get; set; }
    }
}
