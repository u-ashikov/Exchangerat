namespace Exchangerat.Clients.Infrastructure.Extensions
{
    using Data;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.Extensions.DependencyInjection;
    using Services.Contracts.Identity;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<ClientsDbContext>();
            var identityService = serviceProvider.GetService<IIdentityService>();

            SeedExchangeAccountTypes(db);
            SeedClients(db, identityService);
            SeedExchangeAccounts(db);
            SeedTransactions(db);

            return app;
        }

        private static void SeedExchangeAccountTypes(ClientsDbContext db)
        {
            if (db.ExchangeAccountTypes.Any())
            {
                return;
            }

            var accountTypes = new List<ExchangeAccountType>()
            {
                new ExchangeAccountType()
                {
                    Name = "Standard",
                    Description = "An account that is for regular purposes - sending and receiving money."
                },
                new ExchangeAccountType()
                {
                    Name = "Savings",
                    Description = "An account that is used for storing large amount of money for future use."
                },
                new ExchangeAccountType()
                {
                    Name = "Combined",
                    Description = "An account that is used for savings and for regular payments."
                }
            };

            foreach (var exchangeAccountType in accountTypes)
            {
                db.ExchangeAccountTypes.Add(exchangeAccountType);
            }

            db.SaveChanges();
        }

        private static void SeedClients(ClientsDbContext db, IIdentityService identityService)
        {
            if (db.Clients.Count() > 1)
            {
                return;
            }

            var userIds = identityService.GetRegisteredUserIds().GetAwaiter().GetResult();

            var clients = new List<Client>()
            {
                new Client()
                {
                    Address = "Sofia, str. Ivan Vazov 2",
                    FirstName = "Admin",
                    LastName = "Exchangerat"
                },
                new Client()
                {
                    Address = "Varna, str. Chataldja 2",
                    FirstName = "Hristina",
                    LastName = "Ivanova"
                },
                new Client()
                {
                    Address = "Burgas, str. Strandja 10",
                    FirstName = "Koko",
                    LastName = "Kokov"
                },
                new Client()
                {
                    Address = "Sliven, str. Ayshe 22",
                    FirstName = "Kolio",
                    LastName = "Kolev"
                },
                new Client()
                {
                    Address = "Elhovo, str. Kavkaz 1",
                    FirstName = "Pesho",
                    LastName = "Peshev"
                },
                new Client()
                {
                    Address = "Yambol, str. Elena Ivanova 50",
                    FirstName = "Stamat",
                    LastName = "Stamatov"
                }
            };

            for (var i = 0; i < userIds.Count; i++)
            {
                clients[i].UserId = userIds.ElementAt(i);
            }

            db.Clients.AddRange(clients);
            db.SaveChanges();
        }

        private static void SeedExchangeAccounts(ClientsDbContext db)
        {
            if (db.ExchangeAccounts.Any())
            {
                return;
            }

            var clientIds = db.Clients.Select(c => c.Id).ToList();
            var exchangeAccountTypeIds = db.ExchangeAccountTypes.Select(t => t.Id).ToList();

            var random = new Random();

            var exchangeAccounts = new List<ExchangeAccount>()
            {
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT123456" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT714587" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT999158" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT789123" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT333145" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT568254" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT454759" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT456789" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT111567" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT879777" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT222121" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT456781" },
                new ExchangeAccount() { Balance = random.Next(1000, 100000), CreatedAt = DateTime.UtcNow, IdentityNumber = "EXRT998875" }
            };

            foreach (var exchangeAccount in exchangeAccounts)
            {
                var userIndex = random.Next(0, clientIds.Count);
                var accountTypeIndex = random.Next(0, exchangeAccountTypeIds.Count);

                exchangeAccount.OwnerId = clientIds[userIndex];
                exchangeAccount.TypeId = exchangeAccountTypeIds[accountTypeIndex];

                db.ExchangeAccounts.Add(exchangeAccount);
            }

            db.SaveChanges();
        }

        private static void SeedTransactions(ClientsDbContext db)
        {
            if (db.Transactions.Any())
            {
                return;
            }

            var exchangeAccountIds = db.ExchangeAccounts.Select(a => a.Id).ToList();

            var availableDescriptions = new List<string>()
            {
                "Funding of an account",
                "Withdrawing money",
                "Rent for month June 2020",
                "Salary for month June 2020",
                "Car Leasing",
                "Mortgage June 2020",
                "Cat food.",
                "Fantastiko bill 100.99",
            };

            var random = new Random();

            var maxTransactionsCount = 30;

            for (var i = 0; i < maxTransactionsCount; i++)
            {
                var senderIndex = random.Next(0, exchangeAccountIds.Count);
                var receiverIndex = random.Next(0, exchangeAccountIds.Count);
                var descriptionIndex = random.Next(0, availableDescriptions.Count);

                var transaction = new Transaction()
                {
                    SenderAccountId = exchangeAccountIds[senderIndex],
                    ReceiverAccountId = exchangeAccountIds[receiverIndex],
                    Description = availableDescriptions[descriptionIndex],
                    Amount = random.Next(10, 5000),
                    IssuedAt = DateTime.UtcNow
                };

                db.Transactions.Add(transaction);
            }

            db.SaveChanges();
        }
    }
}
