using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelReservationAPI.Models.DTOs.Room
{
    public class RoomReadDto
    {
        public string id { get; set; }
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public bool IsBooked { get; set; }
    }
}
