using System.Text.Json.Serialization;

namespace Domain.Models
{
    using Entities;
    public class AuthenticateResponse
    {
        public string JwtToken { get; set; }

        public string RefreshToken { get; set; }

        public AuthenticateResponse(string jwtToken, string refreshToken)
        {
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}