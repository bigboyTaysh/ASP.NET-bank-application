using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace BankApplication.ViewModels
{
    public class CreationDateGroup
    {
        [DataType(DataType.Date)]
        public DateTime? CreationDate { get; set; }
        public int BankAccountsCount { get; set; }
    }
}