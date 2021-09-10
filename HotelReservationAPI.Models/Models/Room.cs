using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace HotelReservationAPI.Models.Models
{
    [CollectionName("Rooms")]
    public class Room
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public decimal Price { get; set; }
        public int RoomNumber { get; set; }
        public string RoomType { get; set; }
        public bool IsBooked { get; set; } = false;

    }
}
