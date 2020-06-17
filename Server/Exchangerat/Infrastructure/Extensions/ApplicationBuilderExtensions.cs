namespace Exchangerat.Infrastructure.Extensions
{
    using Common.Constants;
    using Data;
    using Data.Enums;
    using Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<ExchangeratDbContext>();

            db.Database.Migrate();

            if (db.Roles.Any() && db.Users.Any(u => u.Email == DataConstants.AdminEmail))
            {
                return app;
            }

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            Task.Run(async () =>
            {
                foreach (var role in Enum.GetNames(typeof(Role)))
                {
                    var roleExists = await roleManager.RoleExistsAsync(role);

                    if (!roleExists)
                    {
                        await roleManager.CreateAsync(new IdentityRole(role));
                    }
                }

                var adminRole = Role.Administrator.ToString();
                var adminUser = await userManager.FindByEmailAsync(DataConstants.AdminEmail);

                if (adminUser == null)
                {
                    adminUser = new User()
                    {
                        Address = DataConstants.AdminAddress,
                        Email = DataConstants.AdminEmail,
                        FirstName = DataConstants.AdminFirstName,
                        LastName = DataConstants.AdminLastName,
                        UserName = DataConstants.AdminUserName
                    };

                    await userManager.CreateAsync(adminUser, DataConstants.AdminPass);

                    await userManager.AddToRoleAsync(adminUser, adminRole);
                }
            })
            .GetAwaiter()
            .GetResult();

            return app;
        }

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<ExchangeratDbContext>();

            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

            SeedExchangeAccountTypes(db);
            SeedUsers(db, userManager);
            SeedExchangeAccounts(db);
            SeedTransactions(db);

            return app;
        }

        private static void SeedExchangeAccountTypes(ExchangeratDbContext db)
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

        private static void SeedUsers(ExchangeratDbContext db, UserManager<User> userManager)
        {
            if (db.Users.Count() > 1)
            {
                return;
            }

            var users = new List<User>()
            {
                new User()
                {
                    Address = "Burgas, str. Strandja 10",
                    Email = "koko@test.bg",
                    FirstName = "Koko",
                    LastName = "Kokov",
                    UserName = "koko"
                },
                new User()
                {
                    Address = "Sliven, str. Ayshe 22",
                    Email = "kolio@test.bg",
                    FirstName = "Kolio",
                    LastName = "Kolev",
                    UserName = "kolio"
                },
                new User()
                {
                    Address = "Yambol, str. Elena Ivanova 50",
                    Email = "stamat@test.bg",
                    FirstName = "Stamat",
                    LastName = "Stamatov",
                    UserName = "stamat"
                },
                new User()
                {
                    Address = "Elhovo, str. Kavkaz 1",
                    Email = "pesho@test.bg",
                    FirstName = "Pesho",
                    LastName = "Peshev",
                    UserName = "pesho"
                },
                new User()
                {
                    Address = "Varna, str. Chataldja 2",
                    Email = "tina@test.bg",
                    FirstName = "Hristina",
                    LastName = "Ivanova",
                    UserName = "hivanova"
                }
            };

            Task.Run(async () =>
            {
                foreach (var user in users)
                {
                    await userManager.CreateAsync(user, "asddsa");

                    await userManager.AddToRoleAsync(user, Role.User.ToString());
                }
            })
            .GetAwaiter()
            .GetResult();
        }

        private static void SeedExchangeAccounts(ExchangeratDbContext db)
        {
            if (db.ExchangeAccounts.Any())
            {
                return;
            }

            var userIds = db.Users.Select(u => u.Id).ToList();
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
                var userIndex = random.Next(0, userIds.Count);
                var accountTypeIndex = random.Next(0, exchangeAccountTypeIds.Count);

                exchangeAccount.OwnerId = userIds[userIndex];
                exchangeAccount.TypeId = exchangeAccountTypeIds[accountTypeIndex];

                db.ExchangeAccounts.Add(exchangeAccount);
            }

            db.SaveChanges();
        }

        private static void SeedTransactions(ExchangeratDbContext db)
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
