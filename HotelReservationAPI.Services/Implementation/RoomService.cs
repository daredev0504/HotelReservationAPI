using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using HotelReservationAPI.Data.Interfaces;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Models.Models;
using HotelReservationAPI.Services.Interfaces;

namespace HotelReservationAPI.Services.Implementation
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IMapper _mapper;

        public RoomService(IRoomRepository roomRepository, IMapper mapper)
        {
            _roomRepository = roomRepository;
            _mapper = mapper;
        }
        public async Task<ServiceResponse<RoomReadDto>> AddRoom(RoomReadDto roomDto)
        {
            var response = new ServiceResponse<RoomReadDto>();

            var roomDomain = _mapper.Map<Room>(roomDto);
            var result = await _roomRepository.AddRoom(roomDomain);

            if (result!= null)
            {
                response.Success = true;
                response.Data = roomDto;
                response.Message = "Room Added Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "An error occurred";
            return response;
        }

        public async Task<ServiceResponse<List<string>>> BookRoom(List<string> ids)
        {
            var response = new ServiceResponse<List<string>>();
            var temp = new List<string>();

            foreach (var id in ids)
            {
                var result = await GetRoomById(id);
                if (result.Success)
                {
                    result.Data.IsBooked = true;

                    var room = _mapper.Map<Room>(result.Data);

                    temp.Add(id);
                    await _roomRepository.UpdateRoom(id, room);

                }
            }
            response.Message = "room(s) booked";
            response.Data = temp;
            return response;

        }


        public async Task<ServiceResponse<bool>> UpdateRoom(string id, RoomUpdateDto roomDto)
        {
            var response = new ServiceResponse<bool>();

            var roomDomain = _mapper.Map<Room>(roomDto);

            var result = await _roomRepository.UpdateRoom(id, roomDomain);

            if (result.IsAcknowledged)
            {
                response.Success = true;
                response.Data = result.IsAcknowledged;
                response.Message = "Room Updated Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "An error occurred";
            return response;
        }

        public async Task<ServiceResponse<bool>> DeleteRoom(string id)
        {
            var response = new ServiceResponse<bool>();

            var result = await _roomRepository.DeleteRoom(id);

            if (result.IsAcknowledged)
            {
                response.Success = true;
                response.Data = result.IsAcknowledged;
                response.Message = "Room deleted Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "An error occurred";
            return response;
        }

        public async Task<ServiceResponse<RoomReadDto>> GetRoomById(string id)
        {
            var response = new ServiceResponse<RoomReadDto>();

            var room = await _roomRepository.GetRoomById(id);

            var roomDomain = _mapper.Map<RoomReadDto>(room);

            if (room != null)
            {
                response.Success = true;
                response.Data = roomDomain;
                response.Message = "Room returned Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "An error occurred";
            return response;
        }

        public async Task<ServiceResponse<IEnumerable<RoomReadDto>>> GetAllRooms()
        {
            var response = new ServiceResponse<IEnumerable<RoomReadDto>>();

            var rooms = await _roomRepository.GetAllRooms();

            var roomsDto = _mapper.Map<IEnumerable<RoomReadDto>>(rooms);

            if (rooms != null)
            {
                response.Success = true;
                response.Data = roomsDto;
                response.Message = "All Rooms returned Successfully";
                return response;
            }
            response.Success = false;
            response.Message = "An error occurred";
            return response;
        }

        public async Task<IEnumerable<RoomReadDto>> GetAvailableRooms()
        {
            var freeRooms = await _roomRepository.GetAllAvailableRooms();
            var rooms = _mapper.Map<IEnumerable<RoomReadDto>>(freeRooms);

            return rooms;
        }

        public async Task<IEnumerable<RoomReadDto>> GetOccupiedRooms()
        {
            var freeRooms = await _roomRepository.GetAllOccupiedRooms();
            var rooms = _mapper.Map<IEnumerable<RoomReadDto>>(freeRooms);

            return rooms;
        }
    }
}
