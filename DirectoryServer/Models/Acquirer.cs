using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DirectoryServer.Models
{
    public class Acquirer
    {
        public int ID { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        public string URL { get; set; }
        public string ApiKey { get; set; }
    }
}