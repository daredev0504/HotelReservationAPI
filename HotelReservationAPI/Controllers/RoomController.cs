using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelReservationAPI.Helper;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace HotelReservationAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RoomController : ControllerBase
    {
        private readonly IRoomService _roomService;

        public RoomController(IRoomService roomService)
        {
            _roomService = roomService;
        }


        /// <summary>
        /// get all rooms in the hotel
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getAllRooms")]
        public async Task<IActionResult> GetAll()
        {
            var response = await _roomService.GetAllRooms();
            if (response.Success)
            {
                return Ok(response);
            }

            return BadRequest(response);
        }

        /// <summary>
        /// get a room by Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("getRoomById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            var response = await _roomService.GetRoomById(id);
            if (response.Success)
            {
                return Ok(response);
               
            }
            return NotFound(response);
        }

        /// <summary>
        /// create a room
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Route("createRoom")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRoom(RoomReadDto model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessage.Message("Invalid Model", ModelState, model));

            var result = await _roomService.AddRoom(model);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// delete a room
        /// </summary>
        /// <returns></returns>
        [HttpDelete]
        [Route("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRoom(string id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessage.Message("Invalid Model", ModelState, id));

            var result = await _roomService.DeleteRoom(id);
            if (result.Success)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        /// <summary>
        /// update an existing room
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("updateRoom/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateRoom(string id, RoomUpdateDto model)
        {
            var queriedRoom = await _roomService.GetRoomById(id);

            if(!queriedRoom.Success)
            {
                return NotFound();
            }
            await _roomService.UpdateRoom(id, model);
            return NoContent();
        }
    }
}
