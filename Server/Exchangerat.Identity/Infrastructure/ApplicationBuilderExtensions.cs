namespace Exchangerat.Identity.Infrastructure
{
    using Data.Enums;
    using Exchangerat.Identity.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using static Exchangerat.Identity.Common.Constants.DataConstants;
    using IdentityDbContext = Data.IdentityDbContext;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();
            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<IdentityDbContext>();

            if (db.Roles.Any() && db.Users.Any(u => u.Email == AdminEmail))
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
                    var adminUser = await userManager.FindByEmailAsync(AdminEmail);

                    if (adminUser == null)
                    {
                        adminUser = new User()
                        {
                            Email = AdminEmail,
                            UserName = AdminUserName
                        };

                        await userManager.CreateAsync(adminUser, AdminPass);

                        await userManager.AddToRoleAsync(adminUser, adminRole);
                    }
                })
                .GetAwaiter()
                .GetResult();

            SeedUsers(db, userManager);

            return app;
        }

        private static void SeedUsers(IdentityDbContext db, UserManager<User> userManager)
        {
            if (db.Users.Count() > 1)
            {
                return;
            }

            var users = new List<User>()
            {
                new User()
                {
                    Email = "koko@test.bg",
                    UserName = "koko"
                },
                new User()
                {
                    Email = "kolio@test.bg",
                    UserName = "kolio"
                },
                new User()
                {
                    Email = "stamat@test.bg",
                    UserName = "stamat"
                },
                new User()
                {
                    Email = "pesho@test.bg",
                    UserName = "pesho"
                },
                new User()
                {
                    Email = "tina@test.bg",
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
    }
}
