using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace HotelReservationAPI.Models.Models
{
    [CollectionName("Reservation")]
    public class Reservation
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
        public DateTime DateBooked { get; set; }
        public decimal AmountToBePaid { get; set; }


        //navigational properties

        [BsonRepresentation(BsonType.String)]
        public Guid GuestId { get; set; }
        public ApplicationUser Guest {get; set;}

        public string GuestEmail { get; set; }

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Rooms {get; set;}

        [BsonIgnore]
        public List<Room> RoomList {get; set;}
    }
}
