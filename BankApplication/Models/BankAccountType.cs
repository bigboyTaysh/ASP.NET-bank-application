using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class BankAccountType
    {
        public int ID { get; set; }
        public decimal Commission { get; set; }
        public string TypeName { get; set; }
    }
}