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
        public string ApiKey { get; set; }
        public string CardNumber { get; set; }
        public string Code { get; set; }
    }
}
