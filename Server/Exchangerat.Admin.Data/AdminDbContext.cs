namespace Exchangerat.Admin.Data
{
    using Exchangerat.Data;
    using Microsoft.EntityFrameworkCore;
    using System.Reflection;

    public class AdminDbContext : MessageDbContext
    {
        public AdminDbContext(DbContextOptions<AdminDbContext> options) 
            : base(options) { }

        protected override Assembly ConfigurationsAssembly => Assembly.GetExecutingAssembly();
    }
}
