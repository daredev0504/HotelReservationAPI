using System.Linq;
using System.Threading.Tasks;
using HotelReservationAPI.Helper;
using HotelReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelReservationAPI.Controllers
{
    /// <summary>
    ///
    /// </summary>
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAppUserService _appUserService;
        private readonly IRoomService _roomService;
        private readonly IReservationService _reservationService;


        public AdminController(IAppUserService appUserService, IRoomService roomService,IReservationService reservationService)
        {
            _appUserService = appUserService;
            _roomService = roomService;
            _reservationService = reservationService;
        }

        /// <summary>
        /// Allows admins to get all User details
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _appUserService.GetUsers();
            return Ok(ResponseMessage.Message("List of all users", null, users));
        }

        /// <summary>
        /// get total rooms
        /// </summary>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getTotalRooms")]
        public async Task<IActionResult> GetTotalRoomsNumber()
        {
            var allrooms = await _roomService.GetAllRooms();

            return Ok(ResponseMessage.Message($"there is a total of {allrooms.Data.Count()} rooms", null, allrooms.Data.ToList()));
        }

        /// <summary>
        /// Allows an admin to get all available rooms
        /// </summary>
        /// <returns>Admin Route</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getAvailableRooms")]
        public async Task<IActionResult> GetAvailableRooms()
        {
            var rooms = await _roomService.GetAvailableRooms();

            return Ok(ResponseMessage.Message($"List of all available rooms {rooms.Count()}", null, rooms));
        }


        /// <summary>
        /// Allows an admin to get all occupied rooms
        /// </summary>
        /// <returns>Admin Route</returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getOccupiedRooms")]
        public async Task<IActionResult> GetOccupiedRooms()
        {
            var rooms = await _roomService.GetOccupiedRooms();

            return Ok(ResponseMessage.Message($"List of all occupied rooms {rooms.Count()}", null, rooms));
        }

     
        /// <summary>
        /// get the rerservations of a Guest
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [Authorize(Roles = "Admin")]
        [HttpGet("getReservationsByUser")]
        public async Task<IActionResult> GetReservationsByUser([FromBody] string email)
        {
            if (!ModelState.IsValid)
                return BadRequest(ResponseMessage.Message("Invalid Model", ModelState));

            var user = await _appUserService.FindAppUserByEmail(email);

            if (user == null)
                return BadRequest(ResponseMessage.Message("Invalid user email", "user with the id was not found", email));

            var bookings = await _reservationService.GetReservationsDetails(email);
            if (bookings == null)
                return NotFound(ResponseMessage.Message("no bookings found", "this user has no reservations", email));


            return Ok(ResponseMessage.Message($"list of reservations by {email} ", null, bookings));
        }
    }
}