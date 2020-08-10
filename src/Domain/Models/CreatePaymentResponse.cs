using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class CreatePaymentResponse
    {
        public bool IsSuccessfull { get; set; }
        public int? PaymentId { get; set; }
        public string ErrorMessage { get; set; }

    }
}
