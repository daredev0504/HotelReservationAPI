namespace HotelReservationAPI.Models.DTOs.AppUser
{
    
    public class AppUserReadDto
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Type { get; set; }

        public string Address { get; set; }
    }
}