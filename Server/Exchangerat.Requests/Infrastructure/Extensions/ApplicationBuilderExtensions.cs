namespace Exchangerat.Requests.Infrastructure.Extensions
{
    using Exchangerat.Requests.Data;
    using Exchangerat.Requests.Data.Models;
    using Exchangerat.Requests.Services.Contracts.Identity;
    using Microsoft.AspNetCore.Builder;
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

            var db = serviceProvider.GetRequiredService<RequestsDbContext>();

            db.Database.Migrate();

            return app;
        }

        public static IApplicationBuilder SeedData(this IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.CreateScope();

            var serviceProvider = serviceScope.ServiceProvider;

            var db = serviceProvider.GetRequiredService<RequestsDbContext>();

            SeedRequesTypes(db);

            var identityService = serviceProvider.GetRequiredService<IIdentityService>();

            SeedRequests(identityService, db);

            return app;
        }

        private static void SeedRequests(IIdentityService identityService, RequestsDbContext db)
        {
            if (db.ExchangeratRequests.Any())
            {
                return;
            }

            Task.Run(async () =>
            {
                var userIds = await identityService.GetRegisteredUsersIds();

                var random = new Random();

                var requestTypeId = db.RequestTypes.FirstOrDefault().Id;

                var requests = new List<ExchangeratRequest>();

                for (int i = 0; i < 10; i++)
                {
                    var status = random.Next(0, 2);
                    var userIdIndex = random.Next(0, userIds.Count-1);

                    var request = new ExchangeratRequest()
                    {
                        RequestTypeId = requestTypeId,
                        IssuedAt = DateTime.UtcNow,
                        Status = (Data.Enums.Status)status,
                        UserId = userIds.ElementAt(userIdIndex)
                    };

                    requests.Add(request);
                }

                db.ExchangeratRequests.AddRange(requests);
                db.SaveChanges();
            })
            .GetAwaiter()
            .GetResult();
        }

        private static void SeedRequesTypes(RequestsDbContext db)
        {
            if (db.RequestTypes.Any())
            {
                return;
            }

            var requestTypes = new List<RequestType>()
            {
                new RequestType() { Type = "Create Account" },
                new RequestType() { Type = "Block Account" },
                new RequestType() { Type = "Delete Account" }
            };

            db.RequestTypes.AddRange(requestTypes);

            db.SaveChanges();
        }
    }
}
