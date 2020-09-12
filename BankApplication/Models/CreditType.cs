using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class CreditType
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public decimal CreditRates { get; set; }
        public decimal Commission { get; set; }
    }
}