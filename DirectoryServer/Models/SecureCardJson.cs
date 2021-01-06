using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace DirectoryServer.Models
{
    [NotMapped]
    public class SecureCardJson
    {
        public string apiKey { get; set; }
        public string cardNumber { get; set; }
        public string code { get; set; }
    }
}
