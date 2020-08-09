using Domain.Data;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Repositories
{
    public class TestBaseRepository
    {
        [Fact]
        public async Task Can_Save()
        {
            //Arrange 
            var merchant = TestHelper.Merchant;

            using (var context = TestHelper.CreateInMemoryDbContext())
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
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = TestHelper.Merchant;
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
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var merchant = TestHelper.Merchant;
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
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var repository = CreateMerchantRepository(context);
                await repository.Add(TestHelper.Merchant);
                var merchant = await repository.GetById(TestHelper.Merchant.Id);

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
            using (var context = TestHelper.CreateInMemoryDbContext())
            {
                //Arrange
                var repository = CreateMerchantRepository(context);
                await repository.Add(TestHelper.Merchant);
                var merchant = await repository.GetById(TestHelper.Merchant.Id);

                //Act
                await repository.Remove(merchant);

                //Assert
                Assert.Empty(context.Merchants);
            }
        }

        private IMerchantRepository CreateMerchantRepository(Database context) => new MerchantRepository(context);


    }
}
