using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Entities
{
    using Helpers;

    public class Payment
    {
        public int Id { get; set; }
        public string CardNumber { get; set; }
        public string NameOnCard { get; set; }
        public string ExpiryDate { get; set; }
        public string IsSuccessfull { get; set; }
        public string BankGatewayIdentifier { get; set; }
        public DateTime CreatedOn { get; set; }
       public decimal Amount { get; set; }
        public int MerchantId { get; set; }
        public Merchant Merchant { get; set; }
    }
}
