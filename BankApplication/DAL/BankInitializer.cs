using BankApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using BankApplication.Controllers;
using BankApplication.Helper;

namespace BankApplication.DAL
{
    public class BankInitializer : DropCreateDatabaseIfModelChanges<BankContext>
    {
        protected override void Seed(BankContext context)
        {
            var roleManager = new RoleManager<IdentityRole>(
                new RoleStore<IdentityRole>(new ApplicationDbContext()));

            var userManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(new ApplicationDbContext()));

            roleManager.Create(new IdentityRole("Admin"));
            roleManager.Create(new IdentityRole("User"));
            roleManager.Create(new IdentityRole("Worker"));

            var user = new ApplicationUser { UserName = "email1@wp.pl" };
            string password = "Password.1";
            userManager.Create(user, password);
            userManager.AddToRole(user.Id, "User");

            var user2 = new ApplicationUser { UserName = "email2@wp.pl" };
            string password2 = "Password.2";
            userManager.Create(user2, password2);
            userManager.AddToRole(user2.Id, "User");

            var user3 = new ApplicationUser { UserName = "admin@wp.pl" };
            string password3 = "Admin.1";
            userManager.Create(user3, password3);
            userManager.AddToRole(user3.Id, "Admin");

            var worker = new ApplicationUser { UserName = "worker@wp.pl" };
            string workerpass = "Worker.1";
            userManager.Create(worker, workerpass);
            userManager.AddToRole(worker.Id, "Worker");

            var currencies = new List<Currency>
            {
                new Currency {
                    Name = "złoty",
                    Code = "PLN", 
                    EffectiveDate = DateTime.Now,
                    Bid = 1.0000m,
                    Ask = 1.0000m
                },
                new Currency {
                    Name = "dolar amerykański",
                    Code = "USD",
                    EffectiveDate = DateTime.Now,
                    Bid = 0.0000m,
                    Ask = 0.0000m
                },
                new Currency {
                    Name = "euro",
                    Code = "EUR",
                    EffectiveDate = DateTime.Now,
                    Bid = 0.0000m,
                    Ask = 0.0000m
                },
                new Currency {
                    Name = "frank szwajcarski",
                    Code = "CHF", 
                    EffectiveDate = DateTime.Now,
                    Bid = 0.0000m,
                    Ask = 0.0000m
                },
                new Currency {
                    Name = "funt szterling",
                    Code = "GBP",
                    EffectiveDate = DateTime.Now,
                    Bid = 0.0000m,
                    Ask = 0.0000m
                },
            };

            currencies.ForEach(c => context.Currencies.Add(c));
            context.SaveChanges();

            var transactionTypes = new List<TransactionType>
            {
                new TransactionType {Type = "TRANSFER"},
                new TransactionType {Type = "CASH_WITHDRAWAL"},
                new TransactionType {Type = "CASH_PAYMENT"}
            };

            transactionTypes.ForEach(t => context.TransactionTypes.Add(t));
            context.SaveChanges();

            var bankAccountTypes = new List<BankAccountType>
            {
                new BankAccountType {Type = "PAY_ACC_FOR_YOUNG", Commission = 0m},
                new BankAccountType {Type = "PAY_ACC_FOR_ADULT", Commission = 5m},
                new BankAccountType {Type = "FOR_CUR_ACC", Commission = 7m}
            };

            bankAccountTypes.ForEach(b => context.BankAccountTypes.Add(b));
            context.SaveChanges();

            var bankAccounts = new List<BankAccount>
            {
                new BankAccount {Balance = 10.50m,
                    AvailableFounds = 0m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1230",
                    CreationDate = new DateTime(2020, 06, 04),
                    BankAccountType = bankAccountTypes[0],
                    Currency = currencies[0]
                },

                new BankAccount {Balance = 0m,
                    AvailableFounds = 0m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1231",
                    CreationDate = new DateTime(2020, 06, 03),
                    BankAccountType = bankAccountTypes[1],
                    Currency = currencies[0]
                }
            };

            bankAccounts.ForEach(b => context.BankAccounts.Add(b));
            context.SaveChanges();

            var profiles = new List<Profile>
            {
                new Profile { Email = user.UserName, Login = user.UserName, BankAccounts = new List<BankAccount>(){bankAccounts[0]}},
                new Profile { Email = user2.UserName, Login = user2.UserName, BankAccounts = new List<BankAccount>(){bankAccounts[1]}},
                new Profile { Email = user3.UserName, Login = user3.UserName},
                new Profile { Email = worker.UserName, Login = worker.UserName},
            };

            profiles.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();
        }
    }
}