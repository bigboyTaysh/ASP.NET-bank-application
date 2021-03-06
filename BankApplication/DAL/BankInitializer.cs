﻿using BankApplication.Helper;
using BankApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;

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
                    Name = "euro",
                    Code = "EUR",
                    EffectiveDate = DateTime.Now,
                    Bid = 4.4601m,
                    Ask = 4.5503m
                },
                new Currency {
                    Name = "dolar amerykański",
                    Code = "USD",
                    EffectiveDate = DateTime.Now,
                    Bid = 3.6382m,
                    Ask = 3.7118m
                },
                new Currency {
                    Name = "frank szwajcarski",
                    Code = "CHF",
                    EffectiveDate = DateTime.Now,
                    Bid = 4.1106m,
                    Ask = 4.1936m
                },
                new Currency {
                    Name = "funt szterling",
                    Code = "GBP",
                    EffectiveDate = DateTime.Now,
                    Bid = 4.9354m,
                    Ask = 5.0352m
                },
            };

            currencies.ForEach(c => context.Currencies.Add(c));
            context.SaveChanges();

            RefreshCurrency.RefreshCurrenciesAsync().ConfigureAwait(false);

            var transactionTypes = new List<TransactionType>
            {
                new TransactionType {Type = "TRANSFER"},
                new TransactionType {Type = "CARD_PAYMENT"},
                new TransactionType {Type = "CASH_WITHDRAWAL"},
                new TransactionType {Type = "CASH_DEPOSIT"},
                new TransactionType {Type = "CURR_EXCHANGE"},
                new TransactionType {Type = "CREDIT_TRANSFER"},
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

            var creditType = new CreditType { Name = "kredyt gotówkowy", Commission = 8.99m, Rates = 0m };
            context.CreditTypes.Add(creditType);
            context.SaveChanges();

            var bankAccounts = new List<BankAccount>
            {
                new BankAccount {
                    Balance = 100.50m,
                    AvailableFounds = 100.50m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1230",
                    CreationDate = new DateTime(2020, 06, 04),
                    BankAccountType = bankAccountTypes[0],
                    Currency = currencies[0],
                },

                new BankAccount {
                    Balance = 50m,
                    AvailableFounds = 50m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1231",
                    CreationDate = new DateTime(2020, 06, 03),
                    BankAccountType = bankAccountTypes[1],
                    Currency = currencies[0],
                },

                new BankAccount {
                    Balance = 20m,
                    AvailableFounds = 20m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1232",
                    CreationDate = new DateTime(2020, 09, 06),
                    BankAccountType = bankAccountTypes[2],
                    Currency = currencies[1]
                }
            };

            bankAccounts.ForEach(b => context.BankAccounts.Add(b));
            context.SaveChanges();

            var paymentCards = new List<PaymentCard>
            {
                new PaymentCard
                {
                    PaymentCardNumber = "1234 1234 1234 1230",
                    Code = "0321",
                    Blocked = false,
                    SecureCard = true,
                    BankAccount = bankAccounts[0]
                },
                new PaymentCard
                {
                    PaymentCardNumber = "1234 1234 1234 1231",
                    Code = "3021",
                    Blocked = true,
                    SecureCard = false,
                    BankAccount = bankAccounts[1]
                },
                new PaymentCard
                {
                    PaymentCardNumber = "1234 1234 1234 1232",
                    Code = "3201",
                    Blocked = false,
                    SecureCard = true,
                    BankAccount = bankAccounts[2]
                },
            };
            paymentCards.ForEach(p => context.PaymentCards.Add(p));
            context.SaveChanges();

            var profiles = new List<Profile>
            {
                new Profile
                    {
                    FirstName = "John",
                    LastName="Travolta",
                    Email = user.UserName,
                    BankAccounts = new List<BankAccount>(){bankAccounts[0], bankAccounts[2]}
                },
                new Profile
                {
                    FirstName = "John",
                    LastName="Travolta",
                    Email = user2.UserName,
                    BankAccounts = new List<BankAccount>(){bankAccounts[1]}
                },
                new Profile { Email = user3.UserName},
                new Profile { Email = worker.UserName},
            };

            profiles.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();

            var acquirer = new Acquirer()
            {
                Name = "Giga Pizza",
                URL = "https://localhost:44395/",
                OrderDetailsPath = "api/orders/",
                UpdateOrderStatusPath = "api/orders/updateStatus",
                OrderSummaryPath = "summary/",
                Description = "Brak pomysłu na obiad? Zamów pizzę online. Giga Pizza to giga przyjemność!",
                BankAccountNumebr = "52 7949 1333 2906 6136 7434 4779",
                ApiKey = "2a9f86fc-8fd6-439d-99af-30d743180d6a"
            };

            context.Acquirers.Add(acquirer);
            context.SaveChanges();

            var directoryServer = new DirectoryServer()
            {
                Name = "Main Directory Server",
                ApiKey = "06b9e986-9609-4892-933f-9ced84f3e1c8"
            };

            context.DirectoryServers.Add(directoryServer);
            context.SaveChanges();

        }
    }
}