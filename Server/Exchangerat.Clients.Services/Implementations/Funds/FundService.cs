namespace Exchangerat.Clients.Services.Implementations.Funds
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Funds;
    using Exchangerat.Clients.Services.Contracts.Funds;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
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

            if (!account.IsActive)
            {
                return Result.Failure(Messages.ReceiverAccountIsNotActive);
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

        public async Task<Result<IEnumerable<FundOutputModel>>> GetMy(string userId)
        {
            var owner = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<IEnumerable<FundOutputModel>>.Failure(Messages.YouAreNotAClient);
            }

            var funds = await this.dbContext.Funds
                .AsNoTracking()
                .Where(f => f.ClientId == owner.Id)
                .Select(f => new FundOutputModel()
                {
                    CardIdentityNumber = f.CardIdentityNumber,
                    Amount = f.Amount,
                    AccountId = f.AccountId,
                    AccountIdentityNumber = f.Account.IdentityNumber,
                    IssuedAt = f.IssuedAt
                })
                .ToListAsync();

            return Result<IEnumerable<FundOutputModel>>.SuccessWith(funds);
        }
    }
}
