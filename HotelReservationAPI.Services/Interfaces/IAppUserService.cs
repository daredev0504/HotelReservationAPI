using System.Collections.Generic;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.AppUser;
using HotelReservationAPI.Models.Models;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationAPI.Services.Interfaces
{
    public interface IAppUserService
    {
        string GetUserId();
        Task<ServiceResponse<AppUserReadDto>> SignUp(AppUserRegisterDto model);
        Task<ServiceResponse<string>> UpdateUser(ApplicationUser user, AppUserUpdateDto model);
        IEnumerable<AppUserReadDto> GetUsers();
        Task<ServiceResponse<ApplicationUser>> GetUser(string id);
        Task<ServiceResponse<AppUserReadDto>> GetMyDetails();
        Task<ServiceResponse<string>> DeleteUser(string id);
        Task<ApplicationUser> FindAppUserByEmail(string email);
        Task<IdentityResult> AddUserRole(string role);
        Task<IList<string>> GetUserRoles(ApplicationUser user);
        Task<bool> ChangeUserRole(ApplicationUser user, string oldRole, string newRole);
        void AddUserToRole(ApplicationUser user, string role);
        Task<IEnumerable<AppUserReadDto>> GetCheckIns();
        IEnumerable<AppUserReadDto> GetCheckOuts();
        
    }
}
