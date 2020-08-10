using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using WebAPI.Helpers;
using Domain.Repositories;
using Domain.Helpers;

namespace WebApi.Helpers
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtMiddleware> _log;

        public JwtMiddleware(RequestDelegate next, ILogger<JwtMiddleware> log)
        {
            _next = next;
            _log = log;
        }

        public async Task Invoke(HttpContext context,
            ISecurityTokenProvider securityTokenProvider,
            IMerchantRepository merchantRepository)
        {

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token != null)
                await ValidateAndAttachUserToContext(context, securityTokenProvider, merchantRepository, token);

            await _next(context);
        }

        private async Task ValidateAndAttachUserToContext(HttpContext context,
            ISecurityTokenProvider securityTokenProvider,
            IMerchantRepository merchantRepository,
            string token)
        {
            try
            {
                var merchantId = securityTokenProvider.ValidateTokenAndExtactIdentity(token);

                // attach user to context on successful jwt validation
                context.Items["User"] = await merchantRepository.GetById(merchantId);
            }
            catch
            {
                _log.LogWarning($"Token validation failed IP: {context.GetIPAddress()}");
            }
        }
    }
}