using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities
{
    public class RefreshToken
    {
        public string Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedByIP { get; set; }
        public DateTime? RevokedOn { get; set; }
        public string RevokedByIP { get; set; }
        public string ReplacedByToken { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= ExpiresOn;

        internal void SetAsRevoked(string ipAddress, string newToken = null)
        {
            RevokedOn = DateTime.UtcNow;
            RevokedByIP = ipAddress;
            ReplacedByToken = newToken;
        }

        internal static RefreshToken Instantiate(string token, string ipAddress)
        {
            return new RefreshToken
            {
                Token = token,
                ExpiresOn = DateTime.UtcNow.AddDays(7),
                CreatedOn = DateTime.UtcNow,
                CreatedByIP = ipAddress
            };
        }
    }
}