using System.ComponentModel.DataAnnotations;

namespace BankApplication.Models
{
    public class TransactionType
    {
        public int ID { get; set; }
        [Required]
        public string Type { get; set; }
    }
}