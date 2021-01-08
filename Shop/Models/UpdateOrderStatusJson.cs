using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Models
{
    [NotMapped]
    public class UpdateOrderStatusJson
    {
        public int ID { get; set; }
        public string ApiKey { get; set; }
        public bool Status { get; set; }
    }
}
