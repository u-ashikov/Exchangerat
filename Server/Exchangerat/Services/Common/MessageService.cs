namespace Exchangerat.Services.Common
{
    using Data.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Threading.Tasks;

    public class MessageService : DataService<Message>, IMessageService
    {
        public MessageService(DbContext dbContext) 
            : base(dbContext) { }

        public async Task SaveMessage(Message message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            await this.dbContext.AddAsync(message);

            await this.dbContext.SaveChangesAsync();
        }
    }
}
