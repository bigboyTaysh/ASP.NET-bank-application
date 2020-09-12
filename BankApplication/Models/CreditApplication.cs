using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class CreditApplication
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Kwota kredytu")]
        public decimal CreditAmount { get; set; }

        [Display(Name = "Całkowity koszt")]
        public decimal TotalRepayment { get; set; }

        [Display(Name = "Miesięczna rata")]
        public decimal MonthRepayment { get; set; }

        [Display(Name = "Długość kredytu")]
        public int NumberOfMonths { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfSubmission { get; set; }

        [Display(Name = "Stan")]
        public bool? State { get; set; }

        [Required]
        [Display(Name = "Typ kredytu")]
        public int TypeID { get; set; }

        public virtual CreditType Type { get; set; }
    }
}