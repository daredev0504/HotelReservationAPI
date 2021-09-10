using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.AppUser;

namespace HotelReservationAPI.Services.AuthManger
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(AppUserLoginDto userForAuth);
        Task<JwtAuthResult> CreateToken();
        Task<IList<string>> GetRoles(AppUserLoginDto model);
        Task<bool> ConfirmUserEmail(string token, string email);
    }
}
