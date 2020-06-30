namespace Exchangerat.Clients.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;
    using System.Reflection;

    public class ClientsDbContext : DbContext
    {
        public ClientsDbContext(DbContextOptions<ClientsDbContext> options)
            :base(options) { }

        public DbSet<Client> Clients { get; set; }

        public DbSet<ExchangeAccount> ExchangeAccounts { get; set; }

        public DbSet<ExchangeAccountType> ExchangeAccountTypes { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);
        }
    }
}
