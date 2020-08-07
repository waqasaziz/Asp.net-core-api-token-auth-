using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers
{
    public interface IHashingProvider
    {
        string GenerateSalt();
        string GenerateHash(string value, string salt);
        bool Validate(string value, string salt, string hash);
    }

    public class HashingProvider : IHashingProvider
    {
        private readonly HashingOptions _options;

        public HashingProvider(IOptions<HashingOptions> options)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(options));
        }

        public string GenerateSalt()
        {
            var salt = new byte[_options.SaltSize];

            using (var rng = RandomNumberGenerator.Create())
                rng.GetBytes(salt);

            return Convert.ToBase64String(salt);
        }

        public string GenerateHash(string value, string salt)
        {
            var hash = KeyDerivation.Pbkdf2(
                          password: value,
                          salt: Encoding.UTF8.GetBytes(salt),
                          prf: _options.Prf,
                          iterationCount: _options.Iterations,
                          numBytesRequested: _options.KeySize);

            return Convert.ToBase64String(hash);
        }

        public bool Validate(string value, string salt, string hash) => GenerateHash(value, salt) == hash;
    }


}
