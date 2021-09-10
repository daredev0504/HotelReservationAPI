namespace HotelReservationAPI.Helper.MongoSettings
{
    public class MongoDbConfig
    {
        public string Name { get; init; }
        public string Password { get; init; }
        public int Port { get; init; }
        public string RoomsCollectionName { get; init; }
        public string ReservationsCollectionName { get; init; }
        public string ConnectionString => $"mongodb+srv://{Name}:{Password}@cluster0.c9ngp.mongodb.net/test";
    }
}
