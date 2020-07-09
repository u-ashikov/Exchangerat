namespace Exchangerat.Clients.Messages
{
    using Exchangerat.Messages.Admin;
    using MassTransit;
    using Services.Contracts.ExchangeAccounts;
    using System.Threading.Tasks;

    public class AccountCreatedConsumer : IConsumer<AccountCreatedMessage>
    {
        private readonly IExchangeAccountService exchangeAccounts;

        public AccountCreatedConsumer(IExchangeAccountService exchangeAccounts)
        {
            this.exchangeAccounts = exchangeAccounts;
        }

        public async Task Consume(ConsumeContext<AccountCreatedMessage> context)
        {
            var message = context.Message;

            await this.exchangeAccounts.Create(message.UserId);
        }
    }
}
