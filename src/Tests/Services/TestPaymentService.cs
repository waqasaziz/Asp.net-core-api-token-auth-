using APiClient.BankGateway;
using APiClient.BankGateway.DataContracts;
using Castle.Core.Logging;
using Domain.Entities;
using Domain.Helpers;
using Domain.Models;
using Domain.Repositories;
using Domain.Services;
using Microsoft.Extensions.Logging;
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
    public class TestPaymentService
    {

        [Fact]
        public async Task Should_Reject_Payment_With_InValid_Card()
        {
            const string BankIdentifier = "1";
            //Arrange 
            var payment = TestHelper.Payment;
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            paymentRepositoryMock.Setup(x => x.Add(It.IsAny<Payment>())).Returns(Task.FromResult(TestHelper.Payment));

            var bankResposne = new BankGatewayResponse { Identifier = BankIdentifier, Status = BankGatewayPaymentStatus.Successfull };
            var bankGatewayMock = new Mock<IBankGateway>();
            bankGatewayMock.Setup(x => x.SubmitPayment(It.IsAny<BankGatewayRequest>())).Returns(Task.FromResult(bankResposne));

            var logger = Mock.Of<ILogger<PaymentService>>();
            var encryptionProvider = Mock.Of<IEncryptionProvider>();


            var paymentService = new PaymentService(logger, paymentRepositoryMock.Object, bankGatewayMock.Object, encryptionProvider);
            var paymentRequest = Mock.Of<CreatePaymentRequest>();
            var merchant = Mock.Of<Merchant>();

            //Act
            var expected = await paymentService.CreatePayment(paymentRequest, merchant);

            //Assert
            bankGatewayMock.Verify(x => x.SubmitPayment(It.IsAny<BankGatewayRequest>()), Times.Once());
            paymentRepositoryMock.Verify(x => x.Add(It.IsAny<Payment>()), Times.Once());

            Assert.True(expected.IsSuccessfull);
            Assert.Equal(expected.PaymentId, payment.Id);
        }


        [Fact]
        public async Task Should_Create_Payment_With_Valid_Card()
        {
            const string BankIdentifier = "1";
            //Arrange 
            var payment = TestHelper.Payment;
            var paymentRepositoryMock = new Mock<IPaymentRepository>();
            paymentRepositoryMock.Setup(x => x.Add(It.IsAny<Payment>())).Returns(Task.FromResult(TestHelper.Payment));

            var bankResposne = new BankGatewayResponse { Identifier = BankIdentifier, Status = BankGatewayPaymentStatus.Failed };
            var bankGatewayMock = new Mock<IBankGateway>();
            bankGatewayMock.Setup(x => x.SubmitPayment(It.IsAny<BankGatewayRequest>())).Returns(Task.FromResult(bankResposne));

            var logger = Mock.Of<ILogger<PaymentService>>();
            var encryptionProvider = Mock.Of<IEncryptionProvider>();


            var paymentService = new PaymentService(logger, paymentRepositoryMock.Object, bankGatewayMock.Object, encryptionProvider);
            var paymentRequest = Mock.Of<CreatePaymentRequest>();
            var merchant = Mock.Of<Merchant>();

            //Act
            var expected = await paymentService.CreatePayment(paymentRequest, merchant);

            //Assert
            bankGatewayMock.Verify(x => x.SubmitPayment(It.IsAny<BankGatewayRequest>()), Times.Once());
            paymentRepositoryMock.Verify(x => x.Add(It.IsAny<Payment>()), Times.Once());

            Assert.False(expected.IsSuccessfull);
            Assert.Null(expected.PaymentId);
        }
    }
}
