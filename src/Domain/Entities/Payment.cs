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
        public string SecurityCode { get; set; }
        
        [Required]
        [Range(typeof(decimal), 
            ModelConstants.Amount.MinStartingPrice,
            ModelConstants.Amount.MaxStartingPrice,
            ErrorMessage = ModelConstants.Amount.ErrorMessage)]
        public decimal Amount { get; set; }

        [Required]
        public int MerchantId { get; set; }

        public Merchant Merchant { get; set; }

    }
}
