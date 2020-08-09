using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.IdentityModel.Tokens;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Tests.Services
{
    public class TestAuthService
    {

        [Fact]
        public async Task Should_Create_Token_When_Valid_Credentials()
        {
            //Arrange 
            var jwtToken = "Lorem ipsum dolor sit amet";
            var refreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = new Mock<ISecurityTokenProvider>();
            tokenProviderMock.Setup(x => x.GenerateJwtToken(It.IsAny<string>())).Returns(jwtToken);
            tokenProviderMock.Setup(x => x.GenerateRefreshToken(It.IsAny<string>())).Returns(refreshToken);

            var hashingProviderMock = new Mock<IHashingProvider>();
            hashingProviderMock.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string x, string y, string z) => true);

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByUserName(It.IsAny<string>())).Returns(Task.FromResult(TestHelper.Merchant));

            var authService = new AuthService(tokenProviderMock.Object, hashingProviderMock.Object, merchantRepositoryMock.Object);
            var authRequest = Mock.Of<AuthenticateRequest>();

            //Act
            var expected = await authService.Authenticate(authRequest, ipAddress);

            //Assert
            Assert.Equal(expected.JwtToken, jwtToken);
            Assert.Equal(expected.RefreshToken, refreshToken);
        }

        [Fact]
        public async Task Should_Not_Create_Token_When_Invalid_UserName()
        {
            //Arrange 
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = Mock.Of<ISecurityTokenProvider>();

            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByUserName(It.IsAny<string>())).Returns(Task.FromResult((Merchant)null));

            var authService = new AuthService(tokenProviderMock, hashingProviderMock, merchantRepositoryMock.Object);
            var authRequest = new AuthenticateRequest
            {
                Username = TestHelper.Merchant.Username,
                Password = TestHelper.Merchant.PasswordHash,
            };

            //Act
            var expected = await authService.Authenticate(authRequest, ipAddress);

            //Assert
            Assert.Null(expected);
        }

        [Fact]
        public async Task Should_Not_Create_Token_When_Invalid_PassWord()
        {
            //Arrange 
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = Mock.Of<ISecurityTokenProvider>();

            var hashingProviderMock = new Mock<IHashingProvider>();
            hashingProviderMock.Setup(x => x.Validate(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
            .Returns((string x, string y, string z) => false);

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByUserName(It.IsAny<string>())).Returns(Task.FromResult(TestHelper.Merchant));

            var authService = new AuthService(tokenProviderMock, hashingProviderMock.Object, merchantRepositoryMock.Object);
            var authRequest = new AuthenticateRequest
            {
                Username = TestHelper.Merchant.Username,
                Password = TestHelper.Merchant.PasswordHash,
            };

            //Act
            var expected = await authService.Authenticate(authRequest, ipAddress);

            //Assert
            Assert.Null(expected);
        }

        [Fact]
        public async Task Should_Refresh_Token_When_Valid_RefreshToken()
        {
            //Arrange 
            var jwtToken = "Lorem ipsum dolor sit amet";
            var validRefreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var newRefreshToken = "esse cillum dolore eu fugiat nulla pariatur";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = new Mock<ISecurityTokenProvider>();
            tokenProviderMock.Setup(x => x.GenerateJwtToken(It.IsAny<string>())).Returns(jwtToken);
            tokenProviderMock.Setup(x => x.GenerateRefreshToken(It.IsAny<string>())).Returns(newRefreshToken);

            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchant = TestHelper.Merchant;
            var refreshToken = new RefreshToken { Token = validRefreshToken, ExpiresOn = DateTime.UtcNow.AddDays(1) };
            merchant.RefreshTokens.Add(refreshToken);

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByToken(It.IsAny<string>())).Returns(Task.FromResult(merchant));
            merchantRepositoryMock.Setup(x => x.Update(It.IsAny<Merchant>())).Returns(Task.CompletedTask);


            var authService = new AuthService(tokenProviderMock.Object, hashingProviderMock, merchantRepositoryMock.Object);
            var authRequest = Mock.Of<AuthenticateRequest>();

            //Act
            var expected = await authService.RefreshToken(validRefreshToken, ipAddress);

            //Assert
            Assert.Equal(expected.RefreshToken, newRefreshToken);
        }

        [Fact]
        public async Task Should_Invalidate_Old_Refresh_Token_When_Generate_New_RefreshToken()
        {
            //Arrange 
            var jwtToken = "Lorem ipsum dolor sit amet";
            var validRefreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var newRefreshToken = "esse cillum dolore eu fugiat nulla pariatur";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = new Mock<ISecurityTokenProvider>();
            tokenProviderMock.Setup(x => x.GenerateJwtToken(It.IsAny<string>())).Returns(jwtToken);
            tokenProviderMock.Setup(x => x.GenerateRefreshToken(It.IsAny<string>())).Returns(newRefreshToken);

            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchant = TestHelper.Merchant;
            var refreshToken = new RefreshToken { Token = validRefreshToken, ExpiresOn = DateTime.UtcNow.AddDays(1) };
            merchant.RefreshTokens.Add(refreshToken);

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByToken(It.IsAny<string>())).Returns(Task.FromResult(merchant));
            merchantRepositoryMock.Setup(x => x.Update(It.IsAny<Merchant>())).Returns(Task.CompletedTask);


            var authService = new AuthService(tokenProviderMock.Object, hashingProviderMock, merchantRepositoryMock.Object);
            var authRequest = Mock.Of<AuthenticateRequest>();

            //Act
            var expected = await authService.RefreshToken(validRefreshToken, ipAddress);

            //Assert
            Assert.False(refreshToken.IsActive);
            Assert.Equal(refreshToken.RevokedByIP, ipAddress);

        }

        [Fact]
        public async Task Should_Not_Refresh_Token_When_Invalid_RefreshToken()
        {
            //Arrange
            var inValidRefreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = Mock.Of<ISecurityTokenProvider>();
            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByToken(It.IsAny<string>())).Returns(Task.FromResult((Merchant)null));

            var authService = new AuthService(tokenProviderMock, hashingProviderMock, merchantRepositoryMock.Object);

            //Act
            var expected = await authService.RefreshToken(inValidRefreshToken, ipAddress);

            //Assert
            Assert.Null(expected);

        }

        [Fact]
        public async Task Should_Revoke_Token_When_Valid_RefreshToken()
        {
            //Arrange 
            var validRefreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = Mock.Of<ISecurityTokenProvider>();
            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchant = TestHelper.Merchant;
            var refreshToken = new RefreshToken { Token = validRefreshToken, ExpiresOn = DateTime.UtcNow.AddDays(1) };
            merchant.RefreshTokens.Add(refreshToken);

            var merchantRepositoryMock = new Mock<IMerchantRepository>();
            merchantRepositoryMock.Setup(x => x.FindByToken(It.IsAny<string>())).Returns(Task.FromResult(merchant));
            merchantRepositoryMock.Setup(x => x.Update(It.IsAny<Merchant>())).Returns(Task.CompletedTask);

            var authService = new AuthService(tokenProviderMock, hashingProviderMock, merchantRepositoryMock.Object);

            //Act
            var expected = await authService.RevokeToken(validRefreshToken, ipAddress);

            //Assert
            Assert.True(expected);
            Assert.False(refreshToken.IsActive);
        }

        [Fact]
        public async Task Should_Not_Revoke_Token_When_Invalid_RefreshToken()
        {
            //Arrange 
            var InvalidRefreshToken = "ABC";

            var validRefreshToken = "consectetur adipiscing elit, sed do eiusmod";
            var ipAddress = "127.0.0.1";

            var tokenProviderMock = Mock.Of<ISecurityTokenProvider>();
            var hashingProviderMock = Mock.Of<IHashingProvider>();

            var merchant = TestHelper.Merchant;
            var refreshToken = new RefreshToken { Token = validRefreshToken, ExpiresOn = DateTime.UtcNow.AddDays(1) };
            merchant.RefreshTokens.Add(refreshToken);

            var merchantRepositoryMock = Mock.Of<IMerchantRepository>();

            var authService = new AuthService(tokenProviderMock, hashingProviderMock, merchantRepositoryMock);

            //Act
            var expected = await authService.RevokeToken(InvalidRefreshToken, ipAddress);

            //Assert
            Assert.False(expected);
            Assert.True(refreshToken.IsActive);

        }
    }
}
