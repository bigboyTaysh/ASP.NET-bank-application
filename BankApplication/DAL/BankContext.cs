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


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().Property(x => x.Ask).HasPrecision(26, 4);
            modelBuilder.Entity<Currency>().Property(x => x.Bid).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.AvailableFounds).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.Balance).HasPrecision(26, 4);
            modelBuilder.Entity<BankAccount>().Property(x => x.Lock).HasPrecision(26, 4);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
    }
}