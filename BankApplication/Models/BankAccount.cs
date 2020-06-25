using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

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

        [Display(Name = "Bank account number")]
        [MinLength(32)]
        [MaxLength(32)]
        [DisplayFormat(DataFormatString = "{0:## #### #### #### #### #### ####}", ApplyFormatInEditMode = true)]
        public string BankAccountNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime CreationDate { get; set; }

        [Required]
        public int BankAccountTypeID { get; set; }

        public virtual BankAccountType BankAccountType { get; set; }
    }
}