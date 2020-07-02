namespace Exchangerat.Clients.Services.Implementations.ExchangeAccounts
{
    using Data;
    using Exchangerat.Clients.Models.ExchangeAccounts;
    using Exchangerat.Clients.Models.Transactions;
    using Exchangerat.Clients.Services.Contracts.ExchangeAccounts;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExchangeAccountService : IExchangeAccountService
    {
        private readonly ClientsDbContext dbContext;

        public ExchangeAccountService(ClientsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<ICollection<ClientExchangeAccountOutputModel>>> GetMy(string userId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ICollection<ClientExchangeAccountOutputModel>>.Failure("You don't have any exchange accounts!");
            }

            var userAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == owner.Id)
                .AsNoTracking()
                .Select(ea => new ClientExchangeAccountOutputModel()
                {
                    Id = ea.Id,
                    Balance = ea.Balance,
                    AccountNumber = ea.IdentityNumber,
                    Type = ea.Type.Name,
                    IsActive = ea.IsActive,
                    CreatedAt = ea.CreatedAt
                })
                .ToListAsync();

            return Result<ICollection<ClientExchangeAccountOutputModel>>.SuccessWith(userAccounts);
        }

        public async Task<Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>> GetActiveByClientForTransaction(
            string userId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>.Failure("The are no accounts found.");
            }

            var activeUserAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == owner.Id && ea.IsActive)
                .AsNoTracking()
                .Select(ea => new ClientExchangeAccountBaseInfoOutputModel()
                {
                    Id = ea.Id,
                    IdentityNumber = ea.IdentityNumber,
                    Balance = ea.Balance
                })
                .ToListAsync();

            return Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>.SuccessWith(activeUserAccounts);
        }

        public async Task<Result<ExchangeAccountInfoOutputModel>> GetAccountDetails(string userId, int accountId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ExchangeAccountInfoOutputModel>.Failure("Exchange account not found.");
            }

            var exchangeAccount =
                await this.dbContext.ExchangeAccounts
                    .Include(ea => ea.Type)
                    .Include(ea => ea.SentTransactions)
                    .ThenInclude(st => st.ReceiverAccount)
                    .Include(ea => ea.ReceivedTransactions)
                    .ThenInclude(rt => rt.SenderAccount)
                    .FirstOrDefaultAsync(ea => ea.Id == accountId && ea.OwnerId == owner.Id);

            if (exchangeAccount == null)
            {
                return Result<ExchangeAccountInfoOutputModel>.Failure("Exchange account not found!");
            }

            var accountInfo = new ExchangeAccountInfoOutputModel()
            {
                AccountNumber = exchangeAccount.IdentityNumber,
                AccountType = exchangeAccount.Type.Name,
                Balance = exchangeAccount.Balance,
                CreatedAt = exchangeAccount.CreatedAt
            };

            var sentTransactions = exchangeAccount.SentTransactions.Select(t =>
                    new TransactionOutputModel()
                    {
                        SenderAccountNumber = t.SenderAccount.IdentityNumber,
                        ReceiverAccountNumber = t.ReceiverAccount.IdentityNumber,
                        Description = t.Description,
                        Amount = t.Amount,
                        IssuedAt = t.IssuedAt
                    })
                .ToList();

            var receivedTransactions = exchangeAccount.ReceivedTransactions.Select(t =>
                    new TransactionOutputModel()
                    {
                        SenderAccountNumber = t.SenderAccount.IdentityNumber,
                        ReceiverAccountNumber = t.ReceiverAccount.IdentityNumber,
                        Description = t.Description,
                        Amount = t.Amount,
                        IssuedAt = t.IssuedAt
                    })
                .ToList();

            accountInfo.Transactions.AddRange(sentTransactions);
            accountInfo.Transactions.AddRange(receivedTransactions);

            accountInfo.Transactions = accountInfo.Transactions.OrderByDescending(t => t.IssuedAt).ToList();

            return Result<ExchangeAccountInfoOutputModel>.SuccessWith(accountInfo);
        }
    }
}