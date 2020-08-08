using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Helpers
{

    public sealed class TokenOptions
    {
        public const string Token = "Token";

        public string Secret { get; set; }
        public int Size { get; set; } = 64;
        public int JWTExpiryMinutes { get; set; } = 20;

        
    }
}
