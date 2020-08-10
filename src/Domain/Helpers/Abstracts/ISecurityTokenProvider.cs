using Microsoft.IdentityModel.Tokens;

namespace Domain.Helpers
{
    public interface ISecurityTokenProvider
    {
        string ClaimType { get; }
        string GenerateJwtToken(string claimId);
        string GenerateRefreshToken(string ipAddress);

        TokenValidationParameters CreateTokenValidationParameters();

        int ValidateTokenAndExtactIdentity(string token);

    }
}
