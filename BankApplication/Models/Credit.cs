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
        [Display(Name = "Kwota kredytu")]
        public decimal CreditAmount { get; set; }
        [Display(Name = "Całkowity koszt")]
        public decimal TotalRepayment { get; set; }
        [Display(Name = "Spłacono")]
        public decimal CurrentRepayment { get; set; }
        [Display(Name = "Miesięczna rata")]
        public decimal MonthRepayment { get; set; }
        [Display(Name = "Ilość rat")]
        public int NumberOfMonths { get; set; }
        [Display(Name = "Pozostało rat")]
        public int NumberOfMonthsToEnd { get; set; }

        [Display(Name = "Data przyznania")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate { get; set; }

        [Display(Name = "Data spłaty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? EndDate { get; set; }

        [Display(Name = "Data ostatniej wpłaty")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? LastPayment { get; set; }

        [Display(Name = "Spłacono")]
        public bool IsPaidOff { get; set; }
        public virtual CreditType Type { get; set; }
    }
}