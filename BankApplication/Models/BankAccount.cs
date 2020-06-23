using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class BankAccount
    {
        public int ID { get; set; }
        public decimal Balance { get; set; }
        public decimal AvailableFounds { get; set; }
        public decimal Lock { get; set; }
        public string BankAccountNumber { get; set; }
        public DateTime CreationDate { get; set; }
        public int BankAccountTypeID { get; set; }

        public virtual BankAccountType BankAccountType { get; set; }
    }
}