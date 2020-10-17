using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Models
{
    public class BankAccount
    {
        public int ID { get; set; }

        [Display(Name = "Saldo")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal Balance { get; set; }

        [Display(Name = "Dostępne środki")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal AvailableFounds { get; set; }

        [Display(Name = "Blokady")]
        [DisplayFormat(DataFormatString = "{0:0.00}", ApplyFormatInEditMode = true)]
        [Column(TypeName = "money")]
        public decimal Lock { get; set; }

        [Display(Name = "Numer konta")]
        public string BankAccountNumber { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Display(Name = "Data utworzenia")]
        public DateTime CreationDate { get; set; }

        [Required]
        public int BankAccountTypeID { get; set; }

        [Required]
        public int CurrencyID { get; set; }

        public virtual BankAccountType BankAccountType { get; set; }
        public virtual Currency Currency { get; set; }

    }
}
