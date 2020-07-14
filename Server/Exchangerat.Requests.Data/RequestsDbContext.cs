namespace Exchangerat.Requests.Data
{
    using Exchangerat.Data;
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Reflection;

    public class RequestsDbContext : MessageDbContext
    {
        public RequestsDbContext(DbContextOptions<RequestsDbContext> options)
            :base(options) {}

        public DbSet<ExchangeratRequest> ExchangeratRequests { get; set; }

        public DbSet<RequestType> RequestTypes { get; set; }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
