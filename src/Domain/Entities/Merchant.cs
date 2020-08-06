using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    using Helpers;
    public class Merchant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public List<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

        [JsonIgnore]
        public List<Payment> Payments { get; } = new List<Payment>();
    }
}