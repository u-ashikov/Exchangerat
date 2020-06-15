namespace Exchangerat.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class ExchangeratDbContext : IdentityDbContext<User>
    {
        public ExchangeratDbContext(DbContextOptions<ExchangeratDbContext> options)
            :base(options) { }

        public DbSet<ExchangeAccount> ExchangeAccounts { get; set; }

        public DbSet<ExchangeAccountType> ExchangeAccountTypes { get; set; }

        public DbSet<Transaction> Transactions { get; set; }
    }
}
