using Domain.Data;
using Domain.Entities;
using Domain.Helpers;
using Domain.Repositories;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Tests
{
    internal static class TestHelper
    {
        internal const string TokenSecret = "Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod";
      

        internal static Database CreateInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<Database>()
                       .UseInMemoryDatabase(Guid.NewGuid().ToString())
                       .Options;

            return new Database(options);
        }

        internal static Merchant Merchant
        => new Merchant
        {
            Id = 1,
            Name = "Amazon.co.uk",
            Username = "Amazon",
            PasswordSalt = "NZsP6NnmfBuYeJrrAKNuVQ==",
            PasswordHash = "/OOoOer10+tGwTRDTrQSoeCxVTFr6dtYly7d0cPxIak="
        };

        internal static RefreshToken RefreshToken
        => new RefreshToken
        {
            Token = "XYZ",
            CreatedOn = DateTime.Now,
            CreatedByIP = "LocalHost"
        };

        internal static async Task SeedData()
        {
            using (var db = CreateInMemoryDbContext())
            {
                db.Merchants.Add(Merchant);
                await db.SaveChangesAsync();
            }
        }

        internal static IMerchantRepository CreateMerchantRepository(Database context = null)
            => new MerchantRepository(context ?? CreateInMemoryDbContext());

        internal static ISecurityTokenProvider CreateSecurityTokenProvider()
            => new SecurityTokenProvider(Options.Create(new TokenOptions() { Secret = TokenSecret }));

        internal static IHashingProvider CreateHasingProvider()
            => new HashingProvider(Options.Create(new HashingOptions()));

        internal static IAuthService CreateAuthService()
            => new AuthService(CreateSecurityTokenProvider(),
                CreateHasingProvider(),
                (CreateMerchantRepository()));
    }


}
