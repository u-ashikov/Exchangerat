namespace Exchangerat.Clients.IntegrationTests
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        private readonly InMemoryDatabaseRoot dbRoot = new InMemoryDatabaseRoot();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbDescriptor = services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<ClientsDbContext>));

                if (dbDescriptor != null)
                {
                    services.Remove(dbDescriptor);
                }

                services.AddDbContext<ClientsDbContext>(options =>
                {
                    options.UseInMemoryDatabase("ClientsTestDatabase", dbRoot);
                });

                var sp = services.BuildServiceProvider();

                using var scope = sp.CreateScope();

                var db = scope.ServiceProvider.GetRequiredService<ClientsDbContext>();

                if (!db.Database.IsInMemory())
                {
                    throw new Exception("The database is not in memory.");
                }

                db.Database.EnsureCreated();

                this.SeedClients(db);
                this.SeedExchangeAccountTypes(db);
                this.SeedExchangeAccounts(db);
                this.SeedFunds(db);
            });
        }

        private void SeedClients(ClientsDbContext db)
        {
            if (!db.Clients.Any())
            {
                var clients = new List<Client>()
                {
                    new Client()
                    {
                        Id = 1,
                        FirstName = "Pesho",
                        LastName = "Peshov",
                        Address = "Yambol",
                        UserId = "PeshoPeshov",
                        ExchangeAccounts = new List<ExchangeAccount>(),
                        Funds = new List<Fund>()
                    },
                    new Client()
                    {
                        Id = 2,
                        FirstName = "Kiro",
                        LastName = "Kirov",
                        Address = "Sofia",
                        UserId = "KiroKirov",
                        ExchangeAccounts = new List<ExchangeAccount>(),
                        Funds = new List<Fund>()
                    },
                    new Client()
                    {
                        Id = 3,
                        FirstName = "Stamat",
                        LastName = "Stamatov",
                        Address = "Burgas",
                        UserId = "StamatStamatov",
                        ExchangeAccounts = new List<ExchangeAccount>(),
                        Funds = new List<Fund>()
                    }
                };

                db.Clients.AddRange(clients);
                db.SaveChanges();
            }
        }

        private void SeedExchangeAccountTypes(ClientsDbContext db)
        {
            if (!db.ExchangeAccountTypes.Any())
            {
                var exchangeAccountTypes = new List<ExchangeAccountType>()
                {
                    new ExchangeAccountType()
                    {
                        Id = 1,
                        Description = "Test Type",
                        Name = "Test"
                    },
                    new ExchangeAccountType()
                    {
                        Id = 2,
                        Description = "Another Test Type",
                        Name = "Another Test"
                    }
                };

                db.ExchangeAccountTypes.AddRange(exchangeAccountTypes);
                db.SaveChanges();
            }
        }

        private void SeedExchangeAccounts(ClientsDbContext db)
        {
            if (!db.ExchangeAccounts.Any())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == 2);

                var exchangeAccounts = new List<ExchangeAccount>()
                {
                    new ExchangeAccount()
                    {
                        Id = 1,
                        Balance = 100,
                        IdentityNumber = "123456789101",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = true,
                        OwnerId = client.Id,
                        TypeId = 1
                    },
                    new ExchangeAccount()
                    {
                        Id = 2,
                        Balance = 200,
                        IdentityNumber = "123456789109",
                        CreatedAt = DateTime.UtcNow,
                        IsActive = false,
                        ClosedAt = DateTime.UtcNow,
                        OwnerId = client.Id,
                        TypeId = 1
                    }
                };

                db.ExchangeAccounts.AddRange(exchangeAccounts);
                db.SaveChanges();
            }
        }

        private void SeedFunds(ClientsDbContext db)
        {
            if (!db.Funds.Any())
            {
                var client = db.Clients.FirstOrDefault(c => c.Id == 2);
                var account = db.ExchangeAccounts.FirstOrDefault(a => a.OwnerId == client.Id && a.IsActive);

                var funds = new List<Fund>()
                {
                    new Fund()
                    {
                        Id = 1,
                        Amount = 100,
                        AccountId = account.Id,
                        ClientId = client.Id,
                        IssuedAt = DateTime.UtcNow
                    }
                };

                db.Funds.AddRange(funds);
                db.SaveChanges();
            }
        }
    }
}
