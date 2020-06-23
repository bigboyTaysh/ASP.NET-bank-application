using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace BankApplication.DAL
{
    public class BankInitializer : DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var bankAccountTypes = new List<BankAccountType>
            {
                new BankAccountType {TypeName = "Konto dla młodych", Commission = 0m},
                new BankAccountType {TypeName = "Konto dla dorosłych", Commission = 5m}
            };

            bankAccountTypes.ForEach(b => context.BankAccountTypes.Add(b));
            context.SaveChanges();

            var bankAccounts = new List<BankAccount>
            {
                new BankAccount {Balance = 0m, AvailableFounds = 0m, Lock = 0m, BankAccountNumber = "12123412341234123412341230", CreationDate = new DateTime(2020, 06, 04), BankAccountType = bankAccountTypes[0]},
                new BankAccount {Balance = 0m, AvailableFounds = 0m, Lock = 0m, BankAccountNumber = "12123412341234123412341231", CreationDate = new DateTime(2020, 06, 03), BankAccountType = bankAccountTypes[1]}
            };

            bankAccounts.ForEach(b => context.BankAccounts.Add(b));
            context.SaveChanges();
        }
    }
}