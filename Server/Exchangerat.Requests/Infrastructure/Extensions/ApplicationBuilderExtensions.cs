namespace Exchangerat.Requests.Infrastructure.Extensions
{
    using Exchangerat.Requests.Data;
    using Exchangerat.Requests.Data.Models;
    using Microsoft.AspNetCore.Builder;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder Initialize(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<RequestsDbContext>();

            db.Database.Migrate();

            return app;
        }
    }
}
