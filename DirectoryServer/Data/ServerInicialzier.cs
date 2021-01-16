using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DirectoryServer.Models;

namespace DirectoryServer.Data
{
    public class ServerInicialzier
    {
        public static void SeedAsync(ApplicationDbContext context)
        {
            if (!context.Banks.Any())
            {
                var acquirer = new Acquirer()
                {
                    Name = "Giga Pizza",
                    URL = "https://localhost:44395/",
                    ApiKey = "ad777c2b-d332-4107-838a-b37738fa8e1f"
                };

                context.Acquirers.Add(acquirer);
                context.SaveChanges();

                var bank = new Bank()
                {
                    Name = "Bank",
                    URL = "https://localhost:44377/",
                    Path = "paymentCards/cardSecured",
                    ApiKey = "06b9e986-9609-4892-933f-9ced84f3e1c8"
                };

                context.Banks.Add(bank);
                context.SaveChanges();

            }
        }
    }
}
