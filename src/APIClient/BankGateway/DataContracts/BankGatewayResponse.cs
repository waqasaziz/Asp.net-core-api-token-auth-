using System;
using System.Collections.Generic;
using System.Text;

namespace APiClient.BankGateway.DataContracts
{
    public class BankGatewayResponse
    {
        public string CardNumber { get; set; }

        public string NameOnCard { get; set; }

        public string ExpiryDate { get; set; }

        public string SecurityCode { get; set; }

        public decimal Amount { get; set; }
        public string Identifier { get; set; }

        public BankGatewayPaymentStatus Status { get; set; }
    }
}
