using Domain.Helpers;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Controllers;
using Xunit;

namespace Tests.Helpers
{
    public class TestAuthController
    {
        private const int OkResultCode = 200;
        private const int BadRequestResultCode = 400;
        private const int UnauthorizedResultCode = 401;
        private const int NotFoundResultCode = 404;

        [Fact]
        public async Task Should_Authenticate_When_Valid_Credentials()
        {
            // Arrange
            var authResponse = new AuthenticateResponse("XYZ", "ABC");

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.Authenticate(It.IsAny<AuthenticateRequest>(), It.IsAny<string>()))
                           .Returns((AuthenticateRequest x, string y) => Task.FromResult(authResponse));

            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);

            //Act
            var result = (await controller.Authenticate(authRequest)) as OkObjectResult;
            var resultContent = result.Value as AuthenticateResponse;

            // Assert
            Assert.Equal(result.StatusCode, OkResultCode);
            Assert.Equal(resultContent.JwtToken, authResponse.JwtToken);
            Assert.Equal(resultContent.RefreshToken, authResponse.RefreshToken);
        }

        [Fact]
        public async Task Should_Not_Authenticate_BadRequest_When_Invalid_Credentials()
        {
            // Arrange

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.Authenticate(It.IsAny<AuthenticateRequest>(), It.IsAny<string>()))
                           .Returns((AuthenticateRequest x, string y) => Task.FromResult((AuthenticateResponse)null));

            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);

            //Act
            var result = (await controller.Authenticate(authRequest)) as BadRequestObjectResult;

            // Assert
            Assert.Equal(result.StatusCode, BadRequestResultCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Should_Refresh_When_Valid_RefreshToken()
        {
            // Arrange
            var refreshToken = "123";
            var authResponse = new AuthenticateResponse("XYZ", "ABC");

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns((string x, string y) => Task.FromResult(authResponse));


            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);
            //Act
            var result = (await controller.RefreshToken(refreshToken)) as OkObjectResult;
            var resultContent = result.Value as AuthenticateResponse;

            // Assert
            Assert.Equal(result.StatusCode, OkResultCode);
            Assert.Equal(resultContent.JwtToken, authResponse.JwtToken);
            Assert.Equal(resultContent.RefreshToken, authResponse.RefreshToken);
        }

        [Fact]
        public async Task Should_Not_Refresh_When_Invalid_RefreshToken()
        {
            // Arrange
            var refreshToken = "123";

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RefreshToken(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns((string x, string y) => Task.FromResult((AuthenticateResponse)null));

            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);

            //Act
            var result = (await controller.RefreshToken(refreshToken)) as UnauthorizedObjectResult;

            // Assert
            Assert.Equal(result.StatusCode, UnauthorizedResultCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Should_Revoke_token_When_Valid_RefreshToken()
        {
            // Arrange
            var request = new RevokeTokenRequest { Token = "ABC" };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RevokeToken(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns((string x, string y) => Task.FromResult(true));


            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);
            //Act
            var result = (await controller.RevokeToken(request)) as OkObjectResult;

            // Assert
            Assert.Equal(result.StatusCode, OkResultCode);
            Assert.NotNull(result.Value);
        }

        [Fact]
        public async Task Should_Not_Revoke_When_Invalid_RefreshToken()
        {
            // Arrange
            var request = new RevokeTokenRequest { Token = "ABC" };

            var authServiceMock = new Mock<IAuthService>();
            authServiceMock.Setup(x => x.RevokeToken(It.IsAny<string>(), It.IsAny<string>()))
                           .Returns((string x, string y) => Task.FromResult(false));


            var authRequest = Mock.Of<AuthenticateRequest>();

            var controller = new AuthController(authServiceMock.Object);
            //Act
            var result = (await controller.RevokeToken(request)) as NotFoundObjectResult;

            // Assert
            Assert.Equal(result.StatusCode, NotFoundResultCode);
            Assert.NotNull(result.Value);
        }


    }
}
