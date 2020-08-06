using System.Text.Json.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Merchant
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Username { get; set; }

        [JsonIgnore]
        public string Password { get; set; }

        [JsonIgnore]
        public ICollection<RefreshToken> RefreshTokens { get; } = new List<RefreshToken>();

        [JsonIgnore]
        public ICollection<Payment> Payments { get; } = new List<Payment>();
    }
}