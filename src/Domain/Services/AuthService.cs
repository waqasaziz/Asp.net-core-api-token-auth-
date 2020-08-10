using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;

namespace Domain.Services
{
    using Models;
    using Entities;
    using Helpers;
    using Repositories;
    using System.Linq;


    public class AuthService : IAuthService
    {
        private readonly ISecurityTokenProvider _tokenProvider;
        private readonly IHashingProvider _hashingProvider;
        private readonly IMerchantRepository _merchantRepository;

        public AuthService(ISecurityTokenProvider tokenService,
            IHashingProvider hashingProvider,
            IMerchantRepository merchantRepository)
        {
            _tokenProvider = tokenService;
            _hashingProvider = hashingProvider;
            _merchantRepository = merchantRepository;
        }

        public async Task<AuthenticateResponse> Authenticate(AuthenticateRequest model, string ipAddress)
        {
            var merchant = await _merchantRepository.FindByUserName(model.Username);
            if (merchant == null) return null;

            if (_hashingProvider.Validate(model.Password, merchant.PasswordSalt, merchant.PasswordHash) == false) return null;

            return await GenerateToken(ipAddress, merchant);
        }

        public async Task<AuthenticateResponse> RefreshToken(string token, string ipAddress)
        {
            var merchant = await _merchantRepository.FindByToken(token);
            if (merchant == null) return null;

            var refreshToken = merchant.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (refreshToken?.IsActive == false) return null;

            return await GenerateToken(ipAddress, merchant, refreshToken);
        }

        public async Task<bool> RevokeToken(string token, string ipAddress)
        {
            var user = await _merchantRepository.FindByToken(token);
            if (user == null) return false;

            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);
            if (refreshToken?.IsActive == false) return false;

            refreshToken.SetAsRevoked(ipAddress);

            await _merchantRepository.Update(user);

            return true;
        }

        private async Task<AuthenticateResponse> GenerateToken(string ipAddress, Merchant merchant, RefreshToken tokenToRevoke = null)
        {
            var token = _tokenProvider.GenerateRefreshToken(ipAddress);

            merchant.RefreshTokens.Add(Entities.RefreshToken.Instantiate(token, ipAddress));

            tokenToRevoke?.SetAsRevoked(ipAddress, token);

            await _merchantRepository.Update(merchant);

            return new AuthenticateResponse(_tokenProvider.GenerateJwtToken(merchant.Id.ToString()), token);
        }

    }
}
