namespace Exchangerat.Services.Common
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class DataService<TEntity> : IDataService<TEntity>
        where TEntity : class
    {
        protected DbContext dbContext { get; }

        protected DataService(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        protected IQueryable<TEntity> All() => this.dbContext.Set<TEntity>();

        public async Task MarkMessageAsPublished(int id)
        {
            var message = await this.dbContext.FindAsync<Message>(id);

            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            message.MarkAsPublished();

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Save(TEntity entity, params Message[] messages)
        {
            foreach (var message in messages)
            {
                this.dbContext.Add(message);
            }

            this.dbContext.Update(entity);

            await this.dbContext.SaveChangesAsync();
        }
    }
}
