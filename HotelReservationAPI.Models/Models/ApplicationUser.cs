using System;
using System.Collections.Generic;
using AspNetCore.Identity.MongoDbCore.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDbGenericRepository.Attributes;

namespace HotelReservationAPI.Models.Models
{
    
    [CollectionName("Users")]
    public class ApplicationUser : MongoIdentityUser<Guid>
    {
        public DateTime DateCreated { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Password { get; set; }
        public string Address { get; set; }

        //navigation properties

        [BsonRepresentation(BsonType.ObjectId)]
        public List<string> Reservations {get; set;}

        [BsonIgnore]
        public List<Reservation> ReservationsList {get; set;}

    }
 
}
