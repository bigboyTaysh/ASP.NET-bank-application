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
        public decimal ValueTo { get; set; }

        [Required]
        [Display(Name = "Wartość")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal ValueFrom { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal BalanceAfterTransactionUserFrom { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal BalanceAfterTransactionUserTo{ get; set; }

        [Display(Name = "Numer konta nadawcy")]
        public string FromBankAccountNumber { get; set; }

        [Required]
        [Display(Name = "Numer konta odbiorcy")]
        public string ToBankAccountNumber { get; set; }

        [Display(Name = "Nazwa nadawcy")]
        public string SenderName { get; set; }

        [Required]
        [Display(Name = "Nazwa odbiorcy")]
        public string ReceiverName { get; set; }

        [Required]
        [Display(Name = "Opis")]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data przelewu")]
        public DateTime Date { get; set; }

        [DataType(DataType.Date)]
        public DateTime OperationDate { get; set; }
        public DateTime OperationD22ate { get; set; }
        public int TransactionTypeID { get; set; }
        public int? CurrencyToID { get; set; }
        public int? CurrencyFromID { get; set; }

        public virtual TransactionType TransactionType { get; set; }
        public virtual Currency CurrencyTo { get; set; }
        public virtual Currency CurrencyFrom { get; set; }
    }
}