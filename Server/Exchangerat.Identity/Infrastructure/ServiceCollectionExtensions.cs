namespace Exchangerat.Identity.Infrastructure
{
    using Data;
    using Exchangerat.Identity.Common.Constants;
    using Exchangerat.Identity.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddUsers(this IServiceCollection services)
        {
            services
                .AddIdentity<User, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequireLowercase = false;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequiredLength = DataConstants.PasswordMinLength;
                    options.User.RequireUniqueEmail = true;
                })
                .AddEntityFrameworkStores<IdentityDbContext>();

            return services;
        }
    }
}
