using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Repositories;
using Domain.Services;
using APiClient.BankGateway;
using WebApi.Helpers;
using Domain.Helpers;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Domain.Entities;

namespace WebAPI.Helpers
{
    public static class Extensions
    {
        public const string MerchantContextKey = "Merchant";

        public static Merchant GetMerchant(this HttpContext context) 
        {
            if (context == null) return null;

            return (Merchant)context.Items[MerchantContextKey];
        }

        public static string GetIPAddress(this HttpContext context)
        {
            if (context == null) return string.Empty;

            if (context.Request.Headers.ContainsKey("X-Forwarded-For"))
                return context.Request.Headers["X-Forwarded-For"];
            else
                return context.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }

        public static void AddAppServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.Configure<HashingOptions>(Configuration.GetSection(HashingOptions.Hashing));
            services.Configure<TokenOptions>(Configuration.GetSection(TokenOptions.Token));

            services.AddScoped<IHashingProvider, PKDF2HashingProvider>();
            services.AddSingleton<IEncryptionKeyProvider, ConfigFileKeyProvider>();
            services.AddSingleton<IEncryptionProvider>(provider =>
            {
                var keyProvider = provider.GetRequiredService<IEncryptionKeyProvider>();
                return new RSAEncryptionProvider(RSAType.RSA2, Encoding.UTF8, keyProvider);
            });

            services.AddScoped<IBankGateway, BankGatewayMock>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IPaymentService, PaymentService>();

            services.AddScoped<ISecurityTokenProvider, SecurityTokenProvider>();
            services.AddScoped<IMerchantRepository, MerchantRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();
        }

        public static void AddJWTAuthentication(this IServiceCollection services, IConfiguration Configuration)
        {
            var appSettingsSection = Configuration.GetSection(TokenOptions.Token);
            var tokenOptions = appSettingsSection.Get<TokenOptions>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(tokenOptions.Secret)),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }
    }
}
