using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Domain.Models
{
    public class GetPaymentsResponse
    {
        public string CardNumber { get; set; }

        public string NameOnCard { get; set; }

        public string ExpiryDate { get; set; }

        public string SecurityCode { get; set; }
        
        public decimal Amount { get; set; }
        
        public decimal IsSuccessfull { get; set; }
        public DateTime CreatedOn { get; set; }
    }
}
