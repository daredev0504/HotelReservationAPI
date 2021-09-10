using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Models.Models;
using MongoDB.Driver;

namespace HotelReservationAPI.Data.Interfaces
{
    public interface IRoomRepository
    {
        Task<Room> AddRoom(Room room);
        Task<ReplaceOneResult> UpdateRoom(string id, Room room);
        Task<DeleteResult> DeleteRoom(string id);
        Task<Room> GetRoomById(string id);
        Task<IEnumerable<Room>> GetAllRooms();
        Task<IEnumerable<Room>> GetAllAvailableRooms();
        Task<IEnumerable<Room>> GetAllOccupiedRooms();
    }
}
