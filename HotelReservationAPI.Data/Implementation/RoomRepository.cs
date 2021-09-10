using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservationAPI.Data.Interfaces;
using HotelReservationAPI.Helper.MongoSettings;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Models.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace HotelReservationAPI.Data.Implementation
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IMongoCollection<Room> _rooms;
        public IConfiguration Configuration { get; }
        public MongoDbConfig mongoDbSettings { get; set; }

        public RoomRepository(IConfiguration configuration)
        {
            Configuration = configuration;
            mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Name);
            _rooms = database.GetCollection<Room>(mongoDbSettings.RoomsCollectionName);

           
        }


        public async Task<Room> AddRoom(Room room)
        {
            await _rooms.InsertOneAsync(room);
            return room;
        }

        public async Task<ReplaceOneResult> UpdateRoom(string id, Room room)
        {
            var result = await _rooms.ReplaceOneAsync(s => s.Id == id, room);
            return result;
        }

        public async Task<DeleteResult> DeleteRoom(string id)
        {
            var result = await _rooms.DeleteOneAsync(s => s.Id == id);
            return result;
        }

        public async Task<Room> GetRoomById(string id)
        {
            return await _rooms.Find<Room>(s => s.Id == id).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _rooms.Find(s => true).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllAvailableRooms()
        {
            return await _rooms.Find(s => s.IsBooked == false).ToListAsync();
        }

        public async Task<IEnumerable<Room>> GetAllOccupiedRooms()
        {
            return await _rooms.Find(s => s.IsBooked == true).ToListAsync();
        }
    }
}
