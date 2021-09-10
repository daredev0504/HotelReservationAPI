using System.ComponentModel.DataAnnotations;

namespace HotelReservationAPI.Models.DTOs.AppUser
{
    public class AppUserLoginDto
    {
        [Required(ErrorMessage = "Email is required")] 
        public string Email { get; set; }

        [Required(ErrorMessage = "Password name is required")]
        public string Password { get; set; }

    }
}
