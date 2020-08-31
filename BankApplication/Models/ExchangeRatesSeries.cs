using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class ExchangeRatesSeries
    {
        public string Table { get; set; }
        public string Currency { get; set; }
        public string Code { get; set; }
        public Rates [] Rates { get; set; }

    }

    public class Rates
    {
        public string No { get; set; }
        public DateTime EffectiveDate { get; set; }
        public decimal Bid { get; set; }
        public decimal Ask { get; set; }
    }
}