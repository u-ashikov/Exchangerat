namespace Exchangerat.Identity.IntegrationTests
{
    using Data;
    using Exchangerat.Identity.Data.Models;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc.Testing;
    using Microsoft.AspNetCore.TestHost;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class CustomWebApplicationFactory<TStartup> : WebApplicationFactory<Startup>
        where TStartup : class
    {
        private readonly InMemoryDatabaseRoot _dbRoot = new InMemoryDatabaseRoot();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                var dbDescriptor =
                    services.SingleOrDefault(s => s.ServiceType == typeof(DbContextOptions<IdentityDbContext>));

                if (dbDescriptor != null)
                {
                    services.Remove(dbDescriptor);
                }

                services.AddDbContext<IdentityDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase", _dbRoot);
                });

                this.InitDatabase(services);
            });
        }

        private void InitDatabase(IServiceCollection services)
        {
            var sp = services.BuildServiceProvider();

            using var scope = sp.CreateScope();

            var db = scope.ServiceProvider.GetRequiredService<IdentityDbContext>();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            if (!db.Database.IsInMemory())
            {
                throw new Exception("The database is not in memory.");
            }

            db.Database.EnsureCreated();

            if (!db.Roles.Any())
            {
                this.SeedRoles(db, roleManager);
            }

            if (!db.Users.Any())
            {
                this.SeedUsers(db, userManager);
            }
        }

        private void SeedRoles(IdentityDbContext db, RoleManager<IdentityRole> roleManager)
        {
            var roles = new List<string>()
            {
                "Administrator",
                "User"
            };

            foreach (var role in roles)
            {
                var result = roleManager.CreateAsync(new IdentityRole(role)).Result;

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }
            }
        }

        private void SeedUsers(IdentityDbContext db, UserManager<User> userManager)
        {
            var users = new List<User>()
            {
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@admin.com",
                    UserName = "admin"
                },
                new User()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "normal@normal.com",
                    UserName = "Normal"
                }
            };

            foreach (var user in users)
            {
                var result = userManager.CreateAsync(user, "asddsa").Result;

                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(Environment.NewLine, result.Errors.Select(e => e.Description)));
                }

                IdentityResult roleResult;

                if (user.UserName == "admin")
                {
                    roleResult = userManager.AddToRoleAsync(user, "Administrator").Result;
                }
                else
                {
                    roleResult = userManager.AddToRoleAsync(user, "User").Result;
                }
            }
        }
    }
}
