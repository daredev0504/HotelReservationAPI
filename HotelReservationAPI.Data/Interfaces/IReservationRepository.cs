using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservationAPI.Models.Models;
using MongoDB.Driver;

namespace HotelReservationAPI.Data.Interfaces
{
    public interface IReservationRepository
    {
        Task<Reservation> AddReservation(Reservation reservation);
        //Task<ReplaceOneResult> UpdateReservation(string id, Room room);
        Task<DeleteResult> DeleteReservation(string email, string reservationId);
        Task<List<Reservation>> GetYourReservations(string email);
        Task<List<Reservation>> GetReservationByIdForAdmin(string email);
        Task<Reservation> GetReservationById(string email, string id);
        //Task<IEnumerable<Reservation>> GetAllReservation();

        string GetUserId();
    }
}
