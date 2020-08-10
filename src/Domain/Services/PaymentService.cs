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
    using System.Runtime.InteropServices.ComTypes;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public interface IPaymentService
    {
        Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request, Merchant merchant);
        Task<IEnumerable<GetPaymentsResponse>> GetAll(Merchant merchant);
    }
    public class PaymentService : IPaymentService
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
        public async Task<CreatePaymentResponse> CreatePayment(CreatePaymentRequest request, Merchant merchant)
        {
            try
            {
                if (merchant == null) return null;

                _log.LogInformation($"Requesting bank for merchant {merchant.Id} : {request.Amount:C2}");

                var bankResponse = await _bankGateway.SubmitPayment(PaymentMapper.MapGatewayRequest(request));

                _log.LogInformation($"Bank response for merchant {merchant.Id} : {bankResponse.Status} ");



                var payment = await _paymentRepository.Add(PaymentMapper.MapPayment(bankResponse, merchant, _encryptionProvier));

                var isPaymentSuccessfull = bankResponse.Status == BankGatewayPaymentStatus.Successfull;
                var result = new CreatePaymentResponse { IsSuccessfull = isPaymentSuccessfull };

                if (isPaymentSuccessfull)
                    result.PaymentId = payment.Id;
                else
                    result.ErrorMessage = "Unable to process payment with Bank";


                _log.LogInformation($"Created  Payment for  merchant {merchant.Id} : {result.PaymentId} ");

                return result;

            }
            catch (Exception ex)
            {
                _log.LogError("Unabl to process paument", ex);

                return new CreatePaymentResponse
                {
                    IsSuccessfull = false,
                    ErrorMessage = "An unexpected error occured, please try again later or contact support"
                };
            }
        }

        public async Task<IEnumerable<GetPaymentsResponse>> GetAll(Merchant merchant)
        {
            if (merchant == null) return null;

            var result = await _paymentRepository.GetAllByMerchant(merchant);
            return result.Select(x => PaymentMapper.MapGetPaymentsResponse(x, _encryptionProvier));
        }
    }
}
