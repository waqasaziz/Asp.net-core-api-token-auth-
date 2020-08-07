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
        public async Task Can_Save()
        {
            //Arrange 
            var merchant = Merchant;

            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);

                //Assert
                Assert.Single(context.Merchants);
            }
        }

        [Fact]
        public async Task Can_Find_By_Id()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = Merchant;
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);

                //Act
                var result = await repository.GetById(merchant.Id);

                //Assert
                Assert.NotNull(result);
            }
        }

        [Fact]
        public async Task Can_Find_By_UserName()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = Merchant;
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);

                //Act
                var result = await repository.FindByUserName(merchant.Username);

                //Assert
                Assert.Equal(result, merchant);
            }
        }

        [Fact]
        public async Task Can_Update()
        {
            var newMerchantName = "Ebay.co.uk";
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var repository = CreateMerchantRepository(context);
                await repository.Add(Merchant);
                var merchant = await repository.GetById(Merchant.Id);

                //Act
                merchant.Name = newMerchantName;
                await repository.Update(merchant);

                //Assert
                Assert.Equal(context.Merchants.First().Name, newMerchantName);
            }
        }

        [Fact]
        public async Task Can_Delete()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var repository = CreateMerchantRepository(context);
                await repository.Add(Merchant);
                var merchant = await repository.GetById(Merchant.Id);

                //Act
                await repository.Remove(merchant);

                //Assert
                Assert.Empty(context.Merchants);
            }
        }

        [Fact]
        public async Task Can_Save_Refresh_Token()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = Merchant;
                merchant.RefreshTokens.Add(RefreshToken);
                
                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.GetById(Merchant.Id);


                //Assert
                Assert.Single(result.RefreshTokens);
            }
        }

        [Fact]
        public async Task Can_Find_By_Token()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = Merchant;
                merchant.RefreshTokens.Add(RefreshToken);

                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.FindByToken(RefreshToken.Token);


                //Assert
                Assert.Equal(result, merchant);
            }
        }

        [Fact]
        public async Task ShouldReturnNull_WithInvalidUserName()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = Merchant;
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);

                //Act
                var result = await repository.FindByUserName("ABC");

                //Assert
                Assert.Null(result);
            }
        }

        [Fact]
        public async Task ShouldReturnNull_WithInvalidToken()
        {
            using (var context = TestsHelper.CreateInMemoryDbContext())
            {
                //Arrange 
                var merchant = Merchant;
                merchant.RefreshTokens.Add(RefreshToken);

                //Act
                var repository = CreateMerchantRepository(context);
                await repository.Add(merchant);
                var result = await repository.FindByToken("ABC");


                //Assert
                Assert.Null(result);
            }
        }

        private Merchant Merchant
        {
            get
            {
                return new Merchant
                {
                    Id = 1,
                    Name = "Amazon.co.uk",
                    Username = "Amazon",
                    PasswordSalt = "NZsP6NnmfBuYeJrrAKNuVQ==",
                    PasswordHash = "/OOoOer10+tGwTRDTrQSoeCxVTFr6dtYly7d0cPxIak="
                };
            }
        }

        private RefreshToken RefreshToken
        {
            get
            {
                return new RefreshToken
                {
                    Token = "XYZ",
                    CreatedOn = DateTime.Now,
                    CreatedByIP = "LocalHost"
                };
            }
        }

        private IMerchantRepository CreateMerchantRepository(Database context) => CreateMerchantRepository(context);

    }

}
