using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using HotelReservationAPI.Models.DTOs.AppUser;
using HotelReservationAPI.Models.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace HotelReservationAPI.Services.AuthManger
{
    public class AuthenticationManager : IAuthenticationManager
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApplicationUser _user;

        public AuthenticationManager(UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }

        public async Task<bool> ValidateUser(AppUserLoginDto model)
        {
            _user = await _userManager.FindByEmailAsync(model.Email);
            return (_user != null &&
                    await _userManager.CheckPasswordAsync(_user,
                        model.Password));
        }

        public async Task<bool> ConfirmUserEmail(string token, string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }
               
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                return true;
            }

            return false;
        }

        public async Task<IList<string>> GetRoles(AppUserLoginDto model)
        {
            _user = await _userManager.FindByEmailAsync(model.Email);
            var roles = await _userManager.GetRolesAsync(_user);
            return roles;
        }

        public async Task<JwtAuthResult> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var tokenOptions = GenerateTokenOptions(signingCredentials, claims);
            var accesToken = new JwtSecurityTokenHandler().WriteToken(tokenOptions);
            return new JwtAuthResult()
            {
                AccessToken = accesToken
            };
        }


        private SigningCredentials GetSigningCredentials()
        {
            var key = Encoding.UTF8.GetBytes(Environment.GetEnvironmentVariable("SECRET"));
            var secret = new SymmetricSecurityKey(key);
            return new SigningCredentials(secret,
                SecurityAlgorithms.HmacSha256);
        }


        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email,
                    _user.Email)
            };
            var roles = await _userManager.GetRolesAsync(_user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role,
                    role));
            }

            return claims;
        }



        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var tokenOptions = new JwtSecurityToken(issuer: jwtSettings.GetSection("validIssuer")
                    .Value,
                audience: jwtSettings.GetSection("validAudience")
                    .Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("expires")
                    .Value)),
                signingCredentials: signingCredentials);
            return tokenOptions;
        }
    }
}
