using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime LastPayment { get; set; }
        public bool IsPaidOff { get; set; }
        public CreditType TypeID { get; set; }
        public virtual CreditType Type { get; set; }
    }
}