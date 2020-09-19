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
    }
}
