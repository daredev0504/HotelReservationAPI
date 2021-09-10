using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Models.Models;

namespace HotelReservationAPI.Services.Interfaces
{
    public interface IRoomService
    {
        Task<ServiceResponse<RoomReadDto>> AddRoom(RoomReadDto room);
        Task<ServiceResponse<List<string>>> BookRoom(List<string> ids);
        Task<ServiceResponse<bool>> UpdateRoom(string id, RoomUpdateDto room);
        Task<ServiceResponse<bool>> DeleteRoom(string id);
        Task<ServiceResponse<RoomReadDto>> GetRoomById(string id);
        Task<ServiceResponse<IEnumerable<RoomReadDto>>> GetAllRooms();
        Task<IEnumerable<RoomReadDto>> GetAvailableRooms();
        Task<IEnumerable<RoomReadDto>> GetOccupiedRooms();
    }
}
