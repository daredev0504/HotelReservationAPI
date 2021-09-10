using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using HotelReservationAPI.Helper.MailService;
using HotelReservationAPI.Models.DTOs.AppUser;
using HotelReservationAPI.Models.Models;
using HotelReservationAPI.Services.AuthManger;
using HotelReservationAPI.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using AutoMapper;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace HotelReservationAPI.Services.Implementation
{
    public class AppUserService : IAppUserService
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationManager _authenticationManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailSender _emailSender;
        private readonly ILoggerService _loggerService;
        private readonly IMapper _mapper;
        


        public AppUserService(IServiceProvider serviceProvider,RoleManager<ApplicationRole> roleManager, UserManager<ApplicationUser> userManager, IHttpContextAccessor httpContextAccessor, IEmailSender emailSender)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _authenticationManager = serviceProvider.GetRequiredService<IAuthenticationManager>();
            _httpContextAccessor = httpContextAccessor;
            _emailSender = emailSender;
            _mapper = serviceProvider.GetRequiredService<IMapper>();
            _loggerService = serviceProvider.GetRequiredService<ILoggerService>();
          

        }

        public string GetUserId() => _httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.Email);

        public async Task<ServiceResponse<AppUserReadDto>> SignUp(AppUserRegisterDto model)
        {
            var response = new ServiceResponse<AppUserReadDto>();

            var domainAppUser = _mapper.Map<ApplicationUser>(model);
            domainAppUser.DateCreated = DateTime.Now;

            var result = await _userManager.CreateAsync(domainAppUser, model.Password);

            if (result.Succeeded)
            {

                var message = new MailRequest()
                {
                    Body = @$"Your Registration is Successful, You can now Login",
                    Subject = "Registration Successful",
                    ToEmail = domainAppUser.Email
                };

                await _userManager.AddToRoleAsync(domainAppUser, "Guest");
                var domainAppReadUser = _mapper.Map<AppUserReadDto>(domainAppUser);

                await _emailSender.SendEmailEasyAsync(message);

                response.Success = true;
                response.Data = domainAppReadUser;
                return response;


            }
            else
            {
                response.Message = "A problem occured";
                response.Success = false;
                response.Errors = result.Errors;
                response.Data = null;
                return response;
            }

        }

        public async Task<ServiceResponse<string>> UpdateUser(ApplicationUser user, AppUserUpdateDto model)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            
            if (user == null)
            {
                response.Message = "Sorry! You cannot perform this operation";
                return response;
            }
            user.FirstName = model.FirstName ?? user.FirstName;
            user.LastName = model.LastName ?? user.LastName;
            user.UserName = model.UserName ?? user.UserName;
            user.Email = model.Email ?? user.Email;
            user.PhoneNumber = model.PhoneNumber ?? user.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                response.Message = "Update Successful";
                response.Success = true;
            }
            else
                response.Message = "Could not update user";

            return response;
        }
        public IEnumerable<AppUserReadDto> GetUsers()
        {
            var users = _userManager.Users.ToList();
            var appReadDtoList = _mapper.Map<IEnumerable<AppUserReadDto>>(users);

            return appReadDtoList;
        }

        
        public async Task<ServiceResponse<ApplicationUser>> GetUser(string id)
        {
            ServiceResponse<ApplicationUser> response = new ServiceResponse<ApplicationUser>();

            var user = await _userManager.FindByEmailAsync(id);
            
            if (user == null)
            {
                response.Message = "Sorry! cannot find this user";
                return response;
            }

            response.Message = "User gotten";
            response.Success = true;
            response.Data = user;

            return response;
        }

        public async Task<ServiceResponse<AppUserReadDto>> GetMyDetails()
        {
            ServiceResponse<AppUserReadDto> response = new ServiceResponse<AppUserReadDto>();

            var user = GetUserId();
            var usertoReturn = await GetUser(user);
            if (usertoReturn.Data == null)
            {
                response.Message = "Sorry! cannot find this user";
                return response;
            }

            var userRead = _mapper.Map<AppUserReadDto>(usertoReturn.Data);
            response.Message = "User gotten";
            response.Success = true;
            response.Data = userRead;

            return response;
        }

        public async Task<ApplicationUser> FindAppUserByEmail(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);

            return user;

        }
        public async Task<ServiceResponse<string>> DeleteUser(string id)
        {
            ServiceResponse<string> response = new ServiceResponse<string>();
            var user = await _userManager.FindByIdAsync(id.ToString());

            if (user == null)
            {
                response.Message = "Sorry! You cannot find user";
                return response;
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                response.Message = "User deleted";
                response.Success = true;
                return response;
            }

            else
            {
                response.Message = "A problem occured";
                response.Success = false;
                return response;
            }
        }

        public async Task<IdentityResult> AddUserRole(string role)
        {
            var added = await _roleManager.CreateAsync(new ApplicationRole(){Name = role});

            return added;
        }

        public Task<IList<string>> GetUserRoles(ApplicationUser user) => _userManager.GetRolesAsync(user);

        public async Task<bool> ChangeUserRole(ApplicationUser user, string oldRole, string newRole)
        {
            var removed = await _userManager.RemoveFromRoleAsync(user, oldRole);
            var added = await _userManager.AddToRoleAsync(user, newRole);

            return removed.Succeeded && added.Succeeded;
        }

        public void AddUserToRole(ApplicationUser user, string role)
        {
            _userManager.AddToRoleAsync(user, role);
        }

        public async Task<string> GenerateEmailConfirmationLink(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            return token;
        }
    }
}
