namespace Exchangerat.Clients.Messages
{
    using Exchangerat.Messages.Admin;
    using MassTransit;
    using Services.Contracts.ExchangeAccounts;
    using System.Threading.Tasks;

    public class AccountBlockedConsumer : IConsumer<AccountBlockedMessage>
    {
        private readonly IExchangeAccountService exchangeAccounts;

        public AccountBlockedConsumer(IExchangeAccountService exchangeAccounts)
        {
            this.exchangeAccounts = exchangeAccounts;
        }

        public async Task Consume(ConsumeContext<AccountBlockedMessage> context)
        {
            var message = context.Message;

            await this.exchangeAccounts.Block(message.UserId, message.AccountId);
        }
    }
}
