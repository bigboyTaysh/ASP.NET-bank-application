using BankApplication.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Web;

namespace BankApplication.DAL
{
    public class BankContext : DbContext
    {
        public BankContext() : base("DefaultConnection")
        {
        }
        public DbSet<BankAccountType> BankAccountTypes { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<TransactionType> TransactionTypes { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Currency> Currencies { get; set; }
        public DbSet<CreditType> CreditTypes { get; set; }
        public DbSet<CreditApplication> CreditApplications { get; set; }
        public DbSet<Credit> Credits { get; set; }
        public DbSet<PaymentCard> PaymentCards { get; set; }
        public DbSet<Acquirer> Acquirers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().Property(x => x.Ask).HasPrecision(26, 4);
            modelBuilder.Entity<Currency>().Property(x => x.Bid).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.AvailableFounds).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.Balance).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.Lock).HasPrecision(26, 4);
            modelBuilder.Entity<CreditType>().Property(x => x.Rates).HasPrecision(26, 4);
            modelBuilder.Entity<CreditType>().Property(x => x.Commission).HasPrecision(26, 4);
            modelBuilder.Entity<CreditApplication>().Property(x => x.CreditAmount).HasPrecision(26, 4);
            modelBuilder.Entity<CreditApplication>().Property(x => x.TotalRepayment).HasPrecision(26, 4);
            modelBuilder.Entity<CreditApplication>().Property(x => x.MonthRepayment).HasPrecision(26, 4);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}