namespace Exchangerat.Clients.Messages
{
    using Exchangerat.Messages.Admin;
    using MassTransit;
    using Services.Contracts.ExchangeAccounts;
    using System.Threading.Tasks;

    public class AccountDeletedConsumer : IConsumer<AccountDeletedMessage>
    {
        private readonly IExchangeAccountService exchangeAccounts;

        public AccountDeletedConsumer(IExchangeAccountService exchangeAccounts)
        {
            this.exchangeAccounts = exchangeAccounts;
        }

        public async Task Consume(ConsumeContext<AccountDeletedMessage> context)
        {
            var message = context.Message;

            await this.exchangeAccounts.Delete(message.UserId, message.AccountId);
        }
    }
}
