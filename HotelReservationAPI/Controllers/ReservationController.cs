﻿using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationAPI.Helper;
using HotelReservationAPI.Models.DTOs.Reservation;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservationAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }


        /// <summary>
        /// get all your reservations
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getYourReservations")]
        public async Task<IActionResult> GetYourReservations()
        {
            var response = await _reservationService.GetYourReservations();

            if (response != null)
            {
                return Ok(ResponseMessage.Message("here are your reservations", null, response));
            }

            return BadRequest("error getting your reservations");
        }

      
        /// <summary>
        /// create a reservation
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("createReservations")]
        public async Task<IActionResult> CreateReservations(ReservationBookDto model)
        {
            var response = await _reservationService.AddReservation(model);

            if (response)
            {
                return Ok(ResponseMessage.Message("Reservation booked", null));
            }

            return BadRequest("error booking your reservations");
        }
    }
}
