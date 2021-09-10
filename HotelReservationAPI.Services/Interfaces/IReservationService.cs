using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.Reservation;
using HotelReservationAPI.Models.Models;

namespace HotelReservationAPI.Services.Interfaces
{
    public interface IReservationService
    {
        Task<bool> AddReservation(ReservationBookDto reservation);
        Task<List<Reservation>> GetYourReservations();
        Task<Reservation> GetYourReservationById(string id);
        Task<List<Reservation>> GetReservationsDetails(string id);
        Task<bool> CheckOut(string id);
        Task<bool> CheckIn();
        Task<bool> Pay(string id);
        Task<bool> DeleteReservation(string id);

    }
}
