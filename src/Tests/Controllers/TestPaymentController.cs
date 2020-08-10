using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Domain.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Results;
using WebAPI.Controllers;
using Xunit;

namespace Tests.Helpers
{
    public class TestPaymentController
    {
        private const int OkResultCode = 200;
        private const int BadRequestResultCode = 400;
        private const int UnauthorizedResultCode = 401;
        private const int NotFoundResultCode = 404;

        [Fact]
        public async Task Should_Return_OK_When_Merchant_Identified()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentRequest>(), It.IsAny<Merchant>()))
                           .Returns((CreatePaymentRequest x, Merchant y) => Task.FromResult(new CreatePaymentResponse()));

            var controller = new PaymentsController(paymentServiceMock.Object);

            //Act
            var result = (await controller.Post(Mock.Of<CreatePaymentRequest>())) as OkObjectResult;

            // Assert
            paymentServiceMock.Verify(x => x.CreatePayment(It.IsAny<CreatePaymentRequest>(), It.IsAny<Merchant>()), Times.Once);
            Assert.Equal(result.StatusCode, OkResultCode);
            Assert.IsType<CreatePaymentResponse>(result.Value);
        }

        [Fact]
        public async Task Should_Return_Bad_request_When_Merchant__Not_Identified()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.CreatePayment(It.IsAny<CreatePaymentRequest>(), It.IsAny<Merchant>()))
                           .Returns((CreatePaymentRequest x, Merchant y) => Task.FromResult((CreatePaymentResponse)null));

            var controller = new PaymentsController(paymentServiceMock.Object);

            //Act
            var result = (await controller.Post(Mock.Of<CreatePaymentRequest>())) as BadRequestObjectResult;

            // Assert
            Assert.Equal(result.StatusCode, BadRequestResultCode);
        }


        [Fact]
        public async Task Should_Return_Payments_When_Merchant_Identified()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetAll(It.IsAny<Merchant>()))
                           .Returns((Merchant y) => Task.FromResult(Enumerable.Empty<GetPaymentsResponse>()));

            var controller = new PaymentsController(paymentServiceMock.Object);

            //Act
            var result = (await controller.GetAll()) as OkObjectResult;

            // Assert
            paymentServiceMock.Verify(x => x.GetAll(It.IsAny<Merchant>()), Times.Once);
            Assert.Equal(result.StatusCode, OkResultCode);
        }

        [Fact]
        public async Task Should_Not_Return_Payments_When_Merchant__Not_Identified()
        {
            // Arrange
            var paymentServiceMock = new Mock<IPaymentService>();
            paymentServiceMock.Setup(x => x.GetAll(It.IsAny<Merchant>()))
                           .Returns((Merchant y) => Task.FromResult((IEnumerable<GetPaymentsResponse>)null));

            var controller = new PaymentsController(paymentServiceMock.Object);

            //Act
            var result = (await controller.GetAll()) as BadRequestObjectResult;

            // Assert
            paymentServiceMock.Verify(x => x.GetAll(It.IsAny<Merchant>()), Times.Once);
            Assert.Equal(result.StatusCode, BadRequestResultCode);
        }
    }
}
