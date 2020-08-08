using Domain.Helpers;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Xunit;

namespace Tests.Helpers
{
    public class TestSecurityTokenProvider
    {
        [Fact]
        public void Does_Generate_Valid_Token()
        {
            // Arrange  
            var claimId = "00000000-0000-0000-0000-000000000000";

            var provider = TestHelper.CreateSecurityTokenProvider();
            var token = provider.GenerateJwtToken(claimId);
            var validationParameters = provider.CreateTokenValidationParameters();

            // Act  
            var principal = new JwtSecurityTokenHandler()
                .ValidateToken(token, validationParameters, out _);

            // Assert  
            Assert.Equal(claimId, principal.FindFirst(ClaimTypes.Name).Value);
        }

    }
}
