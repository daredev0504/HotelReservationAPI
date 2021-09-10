using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservationAPI.Data.Interfaces;
using HotelReservationAPI.Helper.MailService;
using HotelReservationAPI.Models.DTOs.Reservation;
using HotelReservationAPI.Models.Models;
using HotelReservationAPI.Services.Interfaces;

namespace HotelReservationAPI.Services.Implementation
{
    public class ReservationService : IReservationService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;
        private readonly IAppUserService _appUserService;
        private readonly IRoomService _roomService;
        private readonly IReservationRepository _reservationRepository;
        private readonly IEmailSender _emailSender;

        public ReservationService(IRoomRepository roomRepository, IMapper mapper, IAppUserService appUserService, IRoomService roomService, IReservationRepository reservationRepository, IEmailSender emailSender)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
            _appUserService = appUserService;
            _roomService = roomService;
            _reservationRepository = reservationRepository;
            _emailSender = emailSender;
        }

        public async Task<bool> AddReservation(ReservationBookDto reservationBookDto)
        {
            var loggedInUser = _appUserService.GetUserId();
            var user = await _appUserService.FindAppUserByEmail(loggedInUser);

            var roomBooked = await _roomService.BookRoom(reservationBookDto.RoomsToBeBooked);


            var totalMoney = await AmountToBePaid(roomBooked.Data);

            var reservation = new Reservation()
            {
                DateBooked = DateTime.Now,
                GuestId = user.Id,
                Rooms = roomBooked.Data,
                AmountToBePaid = totalMoney,
                GuestEmail = user.Email
            };

            var message = new MailRequest()
            {
                Body = $"Hello {user.FirstName} Your Reservation is Successful" +
                       $"Number of room booked - {roomBooked.Data.Count} Total to be paid - {totalMoney}",
                Subject = "Reservation Successful",
                ToEmail = user.Email
            };


            await _reservationRepository.AddReservation(reservation);
            await _emailSender.SendEmailEasyAsync(message);
            return true;
        }

        public async Task<List<Reservation>> GetYourReservations()
        {
            var email = _reservationRepository.GetUserId();
            return  await _reservationRepository.GetYourReservations(email);
        }

        public async Task<Reservation> GetYourReservationById(string id)
        {
            var loggedInUser = _reservationRepository.GetUserId();
            var reservation = await _reservationRepository.GetReservationById(loggedInUser, id);

            return reservation;
        }

        public async Task<List<Reservation>> GetReservationsDetails(string id)
        {
            var result = await _reservationRepository.GetYourReservations(id);
            return result;
        }


        public async Task<decimal> AmountToBePaid(List<string> roomList)
        {
            decimal totalMoney = 0;

            foreach (var room in roomList)
            {
                var roomDomain = await _roomService.GetRoomById(room);

                totalMoney += roomDomain.Data.Price;
            }

            return totalMoney;
        }

        public async Task<bool> CheckOut()
        {
            //unbook
            //delete reservation
            var roomCurrentlyBooked = await GetYourReservations();
            var tempIds = new List<string>();

            foreach (var reservation in roomCurrentlyBooked)
            {
                 tempIds.AddRange(reservation.Rooms);
            }

            foreach (var room in tempIds)
            {
                var roomDomain = await _roomService.GetRoomById(room);
                roomDomain.Data.IsBooked = false;
                var roomD = _mapper.Map<Room>(roomDomain.Data);
                await _roomRepository.UpdateRoom(roomDomain.Data.id, roomD);
            }
            
        }

        public async Task<bool> DeleteReservation(string id)
        {
            var loggedInUser = _reservationRepository.GetUserId();
            await _reservationRepository.DeleteReservation(loggedInUser, id);
            return true;
        }

       
    }
}
