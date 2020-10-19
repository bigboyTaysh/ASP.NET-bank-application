﻿using BankApplication.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BankApplication.Data
{
    public class BankInitializer
    {
        public static async Task SeedAsync(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            if (!context.Profiles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
                await roleManager.CreateAsync(new IdentityRole("User"));
                await roleManager.CreateAsync(new IdentityRole("Worker"));

                var user = new ApplicationUser
                {
                    UserName = "email1@wp.pl",
                    Email = "email1@wp.pl",
                    LockoutEnabled = false,
                    EmailConfirmed = true
                };
                string password = "Password.1";
                await userManager.CreateAsync(user, password);
                await userManager.AddToRoleAsync(user, "User");
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Name, user.UserName));

                var user2 = new ApplicationUser
                {
                    UserName = "email2@wp.pl",
                    Email = "email2@wp.pl",
                    LockoutEnabled = false,
                    EmailConfirmed = true
                };
                string password2 = "Password.2";
                await userManager.CreateAsync(user2, password2);
                await userManager.AddToRoleAsync(user2, "User");
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Name, user2.UserName));

                var user3 = new ApplicationUser
                {
                    UserName = "admin@wp.pl",
                    Email = "admin@wp.pl",
                    LockoutEnabled = false,
                    EmailConfirmed = true
                };
                string password3 = "Admin.1";
                await userManager.CreateAsync(user3, password3);
                await userManager.AddToRoleAsync(user3, "Admin");
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Name, user3.UserName));

                var user4 = new ApplicationUser
                {
                    UserName = "worker@wp.pl",
                    Email = "worker@wp.pl",
                    LockoutEnabled = false,
                    EmailConfirmed = true
                };
                string workerpass = "Worker.1";
                await userManager.CreateAsync(user4, workerpass);
                await userManager.AddToRoleAsync(user4, "Worker");
                await userManager.AddClaimAsync(user, new System.Security.Claims.Claim(ClaimTypes.Name, user4.UserName));

                var currencies = new List<Currency>
                {
                    new Currency
                    {
                        Name = "złoty",
                        Code = "PLN",
                        EffectiveDate = DateTime.Now,
                        Bid = 1.0000m,
                        Ask = 1.0000m
                    },
                    new Currency
                    {
                        Name = "euro",
                        Code = "EUR",
                        EffectiveDate = DateTime.Now,
                        Bid = 0.0000m,
                        Ask = 0.0000m
                    },
                    new Currency
                    {
                        Name = "dolar amerykański",
                        Code = "USD",
                        EffectiveDate = DateTime.Now,
                        Bid = 0.0000m,
                        Ask = 0.0000m
                    },
                    new Currency
                    {
                        Name = "frank szwajcarski",
                        Code = "CHF",
                        EffectiveDate = DateTime.Now,
                        Bid = 0.0000m,
                        Ask = 0.0000m
                    },
                    new Currency
                    {
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
                    new TransactionType {Type = "CASH_DEPOSIT"},
                    new TransactionType {Type = "CURR_EXCHANGE"},
                    new TransactionType {Type = "CREDIT_TRANSFER"}
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
                new BankAccount {Balance = 50.50m,
                    AvailableFounds = 50.50m,
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
                },

                new BankAccount {Balance = 0m,
                    AvailableFounds = 0m,
                    Lock = 0m,
                    BankAccountNumber = "12 1234 1234 1234 1234 1234 1232",
                    CreationDate = new DateTime(2020, 09, 06),
                    BankAccountType = bankAccountTypes[2],
                    Currency = currencies[1]
                }
                };

                bankAccounts.ForEach(b => context.BankAccounts.Add(b));
                context.SaveChanges();

                var profiles = new List<Profile>
                {
                new Profile
                    {
                    FirstName = "John",
                    LastName="Travolta",
                    Email = user.UserName,
                    Login = user.UserName,
                    BankAccounts = new List<BankAccount>(){bankAccounts[0], bankAccounts[2]}
                },
                new Profile
                {
                    FirstName = "John",
                    LastName="Travolta",
                    Email = user2.UserName,
                    Login = user2.UserName,
                    BankAccounts = new List<BankAccount>(){bankAccounts[1]}
                    },
                    new Profile { Email = user3.UserName, Login = user3.UserName},
                    new Profile { Email = user4.UserName, Login = user4.UserName},
                };

                profiles.ForEach(p => context.Profiles.Add(p));
                context.SaveChanges();

                var transaction = new Transaction
                {
                    ValueTo = 50m,
                    ValueFrom = 50m,
                    BalanceAfterTransactionUserFrom = 0m,
                    BalanceAfterTransactionUserTo = 50.5m,
                    FromBankAccountNumber = "12 1234 1234 1234 1234 1234 1231",
                    ToBankAccountNumber = "12 1234 1234 1234 1234 1234 1230",
                    SenderName = profiles[1].FullName,
                    ReceiverName = profiles[0].FullName,
                    Description = "Przelew pierwszy",
                    Date = DateTime.Now,
                    TransactionType = transactionTypes[0],
                    CurrencyTo = currencies[0],
                    CurrencyFrom = currencies[0]
                };

                context.Transactions.Add(transaction);
                context.SaveChanges();
            }
        }
    }
}