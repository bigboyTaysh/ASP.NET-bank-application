using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class PaymentCard
    {
        public int ID { get; set; }

        [Display(Name = "Numer karty")]
        public string PaymentCardNumber { get; set; }
        [Range(1000, 9999)]
        public int Code { get; set; }
        public bool Blocked { get; set; }
        public bool SecureCard { get; set; }
    }
}