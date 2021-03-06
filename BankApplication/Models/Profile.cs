﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Profile
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PESEL { get; set; }
        public string FullName
        {
            get { return FirstName + " " + LastName; }
        }
        public virtual List<BankAccount> BankAccounts { get; set; }
        public virtual List<CreditApplication> CreditApplications { get; set; }
        public virtual List<Credit> Credits { get; set; }
    }
}