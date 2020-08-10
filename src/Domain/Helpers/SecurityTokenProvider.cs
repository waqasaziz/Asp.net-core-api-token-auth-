using System;
using System.Text;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace Domain.Helpers
{
    using Helpers;
    using Entities;
    using System.Runtime.InteropServices.ComTypes;
    using System.Linq;
    using System.Collections.Generic;


    public class SecurityTokenProvider : ISecurityTokenProvider
    {
        public string ClaimType { get; } = "MerchantID";
        private readonly TokenOptions _options;

        public SecurityTokenProvider(IOptions<TokenOptions> options) => _options = options.Value;

        public string GenerateJwtToken(string claimId)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_options.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimType, claimId)
                }),
                Expires = DateTime.UtcNow.AddMinutes(_options.JWTExpiryMinutes),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            return tokenHandler.WriteToken(tokenHandler.CreateToken(tokenDescriptor));
        }

        public string GenerateRefreshToken(string ipAddress)
        {
            var randomBytes = new byte[_options.Size];

            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
                rngCryptoServiceProvider.GetBytes(randomBytes);

            return Convert.ToBase64String(randomBytes);
        }

        public TokenValidationParameters CreateTokenValidationParameters()
            => new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_options.Secret)),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

        public int ValidateTokenAndExtactIdentity(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.ValidateToken(token, CreateTokenValidationParameters(), out SecurityToken validatedToken);
            var jwtToken = (JwtSecurityToken)validatedToken;
            var idClaim = jwtToken.Claims.First(x => x.Type == ClaimType);
            return int.Parse(idClaim.Value);
        }

    }
}
