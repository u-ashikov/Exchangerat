namespace Exchangerat.Messages
{
    using Data.Models;
    using Hangfire;
    using MassTransit;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    public class MessagesHostedService : IHostedService
    {
        private readonly IServiceScopeFactory scopeFactory;

        private readonly IRecurringJobManager recurringJob;

        private readonly IBus publisher;

        public MessagesHostedService(IRecurringJobManager recurringJob, IBus publisher, IServiceScopeFactory scopeFactory)
        {
            this.recurringJob = recurringJob;
            this.publisher = publisher;
            this.scopeFactory = scopeFactory;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            this.recurringJob
                .AddOrUpdate(nameof(MessagesHostedService),
                    () => this.ProcessPendingMessages(),
                    "*/30 * * * * *");

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public async Task ProcessPendingMessages()
        {
            using var scope = this.scopeFactory.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<DbContext>();

            var messages = dbContext
                .Set<Message>()
                .Where(m => !m.Published)
                .OrderBy(m => m.Id)
                .ToList();

            foreach (var message in messages)
            {
                await this.publisher.Publish(message.Data, message.Type);

                message.MarkAsPublished();

                dbContext.SaveChanges();
            }
        }
    }
}
