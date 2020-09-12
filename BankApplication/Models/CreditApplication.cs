﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class CreditApplication
    {
        public int ID { get; set; }
        public decimal CreditAmount { get; set; }
        public decimal TotalRepayment { get; set; }
        public decimal MonthRepayment { get; set; }
        public int NumberOfMonths { get; set; }
        public DateTime DateOfSubmission { get; set; }
        public bool State { get; set; }
        public CreditType TypeID { get; set; }

        public virtual CreditType Type { get; set; }
    }
}