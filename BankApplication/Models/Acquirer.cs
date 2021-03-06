﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.Models
{
    public class Acquirer
    {
        public int ID { get; set; }
        [Display(Name = "Nazwa")]
        public string Name { get; set; }
        public string URL { get; set; }
        [Display(Name = "Szczegóły")]
        public string OrderDetailsPath { get; set; }
        [Display(Name = "Aktualizacja")]
        public string UpdateOrderStatusPath { get; set; }
        [Display(Name = "Podsumowanie")]
        public string OrderSummaryPath { get; set; }
        [Display(Name = "Opis")]
        public string Description { get; set; }
        [Display(Name = "Numer konta")]
        public string BankAccountNumebr { get; set; }
        public string ApiKey { get; set; }
    }
}