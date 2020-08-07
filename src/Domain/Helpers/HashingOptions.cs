using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Helpers
{
    public sealed class HashingOptions
    {
        public const string Hashing = "Hashing";

        public int Iterations { get; set; } = 10000;
        public int SaltSize { get; set; } = 16;
        public int KeySize { get; set; } = 32;
        public KeyDerivationPrf Prf { get; set; } = KeyDerivationPrf.HMACSHA256;

    }
}
