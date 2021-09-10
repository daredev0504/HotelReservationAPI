using AutoMapper;
using HotelReservationAPI.Models.DTOs.AppUser;
using HotelReservationAPI.Models.DTOs.Room;
using HotelReservationAPI.Models.Models;

namespace HotelReservationAPI.Profiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // source => target
            //appUser
            
            CreateMap<ApplicationUser, AppUserReadDto>()
                .ForMember(c => c.Name, opt =>
                    opt.MapFrom(x => string.Join(' ', x.FirstName, x.LastName)));
            CreateMap<ApplicationUser, AppUserUpdateDto>();
            CreateMap<AppUserUpdateDto, ApplicationUser>();
            CreateMap<AppUserRegisterDto, ApplicationUser>();


           
            //Room
            CreateMap<Room, RoomReadDto>();
            CreateMap<RoomReadDto, Room>();
            //CreateMap<WalletCreateDto, Wallet>();
            //CreateMap<Wallet, WalletCreateDto>();
            //CreateMap<WalletUpdateDto,Wallet>();
            //CreateMap<Wallet, WalletUpdateDto>();

        }
    }
}
