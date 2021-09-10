using System.ComponentModel.DataAnnotations;

namespace HotelReservationAPI.Models.DTOs.AppUser
{
    public class AppUserRegisterDto
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        [MaxLength(255)]
        public string Address { get; set; }
    }
}
