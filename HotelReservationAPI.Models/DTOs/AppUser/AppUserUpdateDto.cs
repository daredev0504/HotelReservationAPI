using System.ComponentModel.DataAnnotations;

namespace HotelReservationAPI.Models.DTOs.AppUser
{
    public class AppUserUpdateDto
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Username is required")] 
        public string UserName { get; set; }

        [Required(ErrorMessage = "Password is required")] 
        public string Password { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
