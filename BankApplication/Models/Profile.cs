using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string Login { get; set; }
        public virtual List<BankAccount> BankAccounts { get; set; }
    }
}