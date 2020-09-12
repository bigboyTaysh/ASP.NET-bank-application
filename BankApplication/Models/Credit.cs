using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Credit
    {
        public int ID { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TotalRepayment { get; set; }
        public decimal CurrentRepayment { get; set; }
        public decimal MonthRepayment { get; set; }
        public int NumberOfMonths { get; set; }
        public int NumberOfMonthsToEnd { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime LastPayment { get; set; }
        public bool IsPaidOff { get; set; }
        public CreditType TypeID { get; set; }

        public virtual CreditType Type { get; set; }
    }
}