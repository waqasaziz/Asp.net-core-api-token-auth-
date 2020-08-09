using Domain.Data;
using Domain.Entities;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Repositories
{

    public class TestMerchantRepository
    {
        [Fact]
        public async Task Can_Save_Refresh_Token()
        {
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = TestHelper.Merchant;
                merchant.RefreshTokens.Add(TestHelper.RefreshToken);
                
                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.GetById(merchant.Id);


                //Assert
                Assert.Single(result.RefreshTokens);
            }
        }

        [Fact]
        public async Task Can_Find_By_Token()
        {
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = TestHelper.Merchant;
                var refreshToken = TestHelper.RefreshToken;
                merchant.RefreshTokens.Add(refreshToken);

                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.FindByToken(refreshToken.Token);

                //Assert
                Assert.Equal(result, merchant);
            }
        }

        [Fact]
        public async Task ShouldReturnNull_WithInvalidUserName()
        {
            var invalidUserName = "ABC";
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = TestHelper.Merchant;
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);

                //Act
                var result = await repository.FindByUserName(invalidUserName);

                //Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ShouldReturnNull_WithInvalidToken()
        {
            var invalidUserName = "ABC";

            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = TestHelper.Merchant;
                merchant.RefreshTokens.Add(TestHelper.RefreshToken);

                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.FindByToken(invalidUserName);

                //Assert
                Assert.Null(result);
            }
        }

        private IMerchantRepository CreateMerchantRepository(Database context) => new MerchantRepository(context);

    }

}
