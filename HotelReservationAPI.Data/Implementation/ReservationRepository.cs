using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelReservationAPI.Data.Interfaces;
using HotelReservationAPI.Helper.MongoSettings;
using HotelReservationAPI.Models.Models;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using Microsoft.AspNetCore.Http;

namespace HotelReservationAPI.Data.Implementation
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IRoomRepository _repository;
        private readonly IMongoCollection<Reservation> _reservations;
        private readonly IHttpContextAccessor _httpContextAccessor;
      
        public IConfiguration Configuration { get; }
        public MongoDbConfig mongoDbSettings { get; set; }

        public ReservationRepository(IRoomRepository repository, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _httpContextAccessor = httpContextAccessor;
            Configuration = configuration;
            mongoDbSettings = Configuration.GetSection(nameof(MongoDbConfig)).Get<MongoDbConfig>();
            var client = new MongoClient(mongoDbSettings.ConnectionString);
            var database = client.GetDatabase(mongoDbSettings.Name);
            _reservations = database.GetCollection<Reservation>(mongoDbSettings.ReservationsCollectionName);
            
        }

        public async Task<List<Reservation>> GetReservationByIdForAdmin(string email)
        {
            return await _reservations.Find<Reservation>(s => s.GuestEmail == email).ToListAsync();
        }

        public async Task<Reservation> GetReservationById(string email, string id)
        {
            return await _reservations.Find<Reservation>(s => s.GuestEmail == email && s.Id == id)
                .FirstOrDefaultAsync();
        }

        public string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);
        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            await _reservations.InsertOneAsync(reservation);
            return reservation;
        }

        public async Task<DeleteResult> DeleteReservation(string email, string reservationId)
        {
            var result = await _reservations.DeleteOneAsync(s => s.Id == reservationId && s.GuestEmail == email);
            return result;
        }

        public async Task<List<Reservation>> GetYourReservations(string email)
        {
            return await _reservations.Find<Reservation>(s => s.GuestEmail == email).ToListAsync();
        }
    }
}
