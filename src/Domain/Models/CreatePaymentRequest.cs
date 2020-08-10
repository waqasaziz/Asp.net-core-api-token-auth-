using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class CreatePaymentRequest
    {
        [Required]
        public string CardNumber { get; set; }

        [Required]
        public string NameOnCard { get; set; }

        [Required]
        public string ExpiryDate { get; set; }

        [Required]
        public string SecurityCode { get; set; }
        
        [Required]
        public decimal Amount { get; set; }
    }
}
