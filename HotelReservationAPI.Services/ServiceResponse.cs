using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace HotelReservationAPI.Services
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; } = false;
        public T Data { get; set; }
        public string Message { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; } = new List<IdentityError>();
    }
}
