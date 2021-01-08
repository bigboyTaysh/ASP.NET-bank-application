using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    [NotMapped]
    public class AcquirerOrderJson
    {
        public int ID { get; set; }
        public decimal Price { get; set; }
        public DateTime Date { get; set; }
        public AcquirerOrderStatus Status { get; set; }
    }

    [NotMapped]
    public class AcquirerOrderStatus
    {
        public int ID { get; set; }
        public string Status { get; set; }
    }
}