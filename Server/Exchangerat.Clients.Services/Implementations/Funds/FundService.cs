namespace Exchangerat.Clients.Services.Implementations.Funds
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Funds;
    using Exchangerat.Clients.Services.Contracts.Funds;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Threading.Tasks;
    using static Exchangerat.Clients.Common.Constants.WebConstants;

    public class FundService : IFundService
    {
        private readonly ClientsDbContext dbContext;

        public FundService(ClientsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result> Add(FundInputModel model, string userId)
        {
            var client = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (client == null)
            {
                return Result.Failure(Messages.YouAreNotAClient);
            }

            var account = await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(ea => ea.Id == model.AccountId && ea.OwnerId == client.Id);

            if (account == null)
            {
                return Result.Failure(Messages.AccountNotFound);
            }

            var fund = new Fund()
            {
                AccountId = account.Id,
                Amount = model.Amount,
                CardIdentityNumber = model.CardIdentityNumber,
                ClientId = client.Id
            };

            account.Balance += model.Amount;

            this.dbContext.Funds.Add(fund);

            await this.dbContext.SaveChangesAsync();

            return Result.Success;
        }
    }
}
