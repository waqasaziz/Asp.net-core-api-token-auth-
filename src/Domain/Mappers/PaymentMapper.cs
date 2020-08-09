using System;
using System.Text;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using APiClient.BankGateway.DataContracts;

[assembly: InternalsVisibleTo("Tests")]
namespace Domain.Mappers
{
    using Models;
    using Entities;
    using Domain.Helpers;

    internal static class PaymentMapper
    {
        internal static Payment MapPayment(BankGatewayResponse response, Merchant merchant, IEncryptionProvider encryptionProvider)
        {
            return new Payment
            {
                CardNumber = encryptionProvider.Encrypt(response.CardNumber),
                NameOnCard = encryptionProvider.Encrypt(response.NameOnCard),
                ExpiryDate = encryptionProvider.Encrypt(response.ExpiryDate),
                Amount = response.Amount,
                CreatedOn = DateTime.UtcNow,
                MerchantId = merchant.Id,
                BankGatewayIdentifier = response.Identifier
            };
        }

        internal static BankGatewayRequest MapGatewayRequest(PaymentRequest request)
        {
            return new BankGatewayRequest
            {
                CardNumber = request.CardNumber,
                NameOnCard = request.NameOnCard,
                ExpiryDate = request.ExpiryDate,
                SecurityCode = request.SecurityCode,
                Amount = request.Amount
            };
        }
    }
}
