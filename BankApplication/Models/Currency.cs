using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Currency
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public DateTime EffectiveDate { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Bid { get; set; }
        
        [DataType(DataType.Currency)]
        public decimal Ask { get; set; }
    }
}