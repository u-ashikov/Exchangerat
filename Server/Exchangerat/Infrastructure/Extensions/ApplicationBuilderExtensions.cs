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
    }
}
