using BankApplication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
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

            var profiles = new List<Profile>
            {
                new Profile { Login = user.UserName, BankAccounts = {bankAccounts[0]}},
                new Profile { Login = user2.UserName, BankAccounts = {bankAccounts[1]}},
                new Profile { Login = user3.UserName, BankAccounts = {bankAccounts[2]}},
                new Profile { Login = worker.UserName, },
            };

            profiles.ForEach(p => context.Profiles.Add(p));
            context.SaveChanges();

            context.SaveChanges();
        }
    }
}