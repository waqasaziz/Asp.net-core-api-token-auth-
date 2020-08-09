using System;
using System.Threading.Tasks;
using APiClient.BankGateway;
using APiClient.BankGateway.DataContracts;
using Microsoft.Extensions.Logging;

namespace Domain.Services
{
    using Helpers;
    using Entities;
    using Mappers;
    using Models;
    using Repositories;

    public class PaymentService
    {
        private readonly ILogger<PaymentService> _log;
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankGateway _bankGateway;
        private readonly IEncryptionProvider _encryptionProvier;

        public PaymentService(ILogger<PaymentService> log,
            IPaymentRepository paymentRepository,
            IBankGateway bankGateway,
            IEncryptionProvider encryptionProvier)
        {
            _log = log;
            _paymentRepository = paymentRepository;
            _bankGateway = bankGateway;
            _encryptionProvier = encryptionProvier;
        }
        public async Task<PaymentResponse> CreatePayment(PaymentRequest request, Merchant merchant)
        {
            _log.LogInformation($"Requesting bank for merchant {merchant.Id} : {request.Amount:C2}");

            var bankResponse = await _bankGateway.SubmitPayment(PaymentMapper.MapGatewayRequest(request));

            _log.LogInformation($"Bank response for merchant {merchant.Id} : {bankResponse.Status} ");

            var result = new PaymentResponse { IsSuccessfull = bankResponse.Status == BankGatewayPaymentStatus.Successfull };

            if (bankResponse.Status == BankGatewayPaymentStatus.Successfull)
            {
                var payment = await _paymentRepository.Add(PaymentMapper.MapPayment(bankResponse, merchant,_encryptionProvier));

                result.PaymentId = payment.Id;
            }

            _log.LogInformation($"Created  Payment for  merchant {merchant.Id} : {result.PaymentId} ");

            return result;
        }


    }
}
