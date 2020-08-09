using System.Text.Json.Serialization;

namespace Domain.Models
{
    using Entities;
    public class AuthenticateResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Username { get; set; }
        public string JwtToken { get; set; }

        [JsonIgnore] // refresh token is returned in http only cookie
        public string RefreshToken { get; set; }

        public AuthenticateResponse(Merchant merchant, string jwtToken, string refreshToken)
        {
            Id = merchant.Id;
            Name = merchant.Name;
            Username = merchant.Username;
            JwtToken = jwtToken;
            RefreshToken = refreshToken;
        }
    }
}