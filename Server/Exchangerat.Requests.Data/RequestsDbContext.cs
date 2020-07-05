namespace Exchangerat.Requests.Data
{
    using Exchangerat.Requests.Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class RequestsDbContext : DbContext
    {
        public RequestsDbContext(DbContextOptions<RequestsDbContext> options)
            :base(options) {}

        public DbSet<ExchangeratRequest> ExchangeratRequests { get; set; }

        public DbSet<RequestType> RequestTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
