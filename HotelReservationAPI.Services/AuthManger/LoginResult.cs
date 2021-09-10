using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace HotelReservationAPI.Services.AuthManger
{
    public class LoginResult
    {
        [JsonPropertyName("username")]
        public string UserName { get; set; }

        [JsonPropertyName("role")]
        public IList<string> Role { get; set; }

        [JsonPropertyName("originalUserName")]
        public string OriginalUserName { get; set; }

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("expiration")]
        public string Expiration { get; set; }
    }
}
