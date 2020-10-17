using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BankApplication.Models
{
    public class TransactionType
    {
        public int ID { get; set; }
        [Required]
        public string Type { get; set; }
    }
}
