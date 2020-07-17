using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;

namespace BankApplication.Models
{
    public class BankAccount
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Balance { get; set; }

        [Required]
        [Display(Name = "Available founds")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal AvailableFounds { get; set; }

        [Required]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        public decimal Lock { get; set; }

        [DataType(DataType.Text)]
        [Display(Name = "Bank account number")]
        [DisplayFormat(DataFormatString = "{0:## #### #### #### #### #### ####}", ApplyFormatInEditMode = true)]
        public decimal BankAccountNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        public int BankAccountTypeID { get; set; }

        public string FileName { get; set; }

        public virtual BankAccountType BankAccountType { get; set; }
    }
}