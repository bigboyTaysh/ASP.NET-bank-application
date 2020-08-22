using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankApplication.Models
{
    public class Transaction
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Wartość")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal Value { get; set; }

        [Required]
        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal BalanceAfterTransactionUserFrom { get; set; }

        [Required]
        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal BalanceAfterTransactionUserTo{ get; set; }

        [Required]
        [Display(Name = "Nadawca")]
        public string FromBankAccountNumber { get; set; }

        [Required]
        [Display(Name = "Odbiorca")]
        public string ToBankAccountNumber { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Creation Date")]
        public DateTime Date { get; set; }

        [Required]
        public int TransactionTypeID { get; set; }

        //public string FileName { get; set; }

        public virtual TransactionType TransactionType { get; set; }
    }
}