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

        public async Task<Result<ICollection<UserExchangeAccountOutputModel>>> GetByUserId(string userId)
        {
            var ownerId = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId).Id;

            var userAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == ownerId)
                .AsNoTracking()
                .Select(ea => new UserExchangeAccountOutputModel()
                {
                    Id = ea.Id,
                    Balance = ea.Balance,
                    AccountNumber = ea.IdentityNumber,
                    Type = ea.Type.Name,
                    IsActive = ea.IsActive,
                    CreatedAt = ea.CreatedAt
                })
                .ToListAsync();

            return Result<ICollection<UserExchangeAccountOutputModel>>.SuccessWith(userAccounts);
        }

        public async Task<Result<ICollection<UserExchangeAccountBaseInfoOutputModel>>> GetActiveByUserForTransaction(
            string userId)
        {
            var ownerId = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId).Id;

            var activeUserAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == ownerId && ea.IsActive)
                .AsNoTracking()
                .Select(ea => new UserExchangeAccountBaseInfoOutputModel()
                {
                    Id = ea.Id,
                    IdentityNumber = ea.IdentityNumber,
                    Balance = ea.Balance
                })
                .ToListAsync();

            return Result<ICollection<UserExchangeAccountBaseInfoOutputModel>>.SuccessWith(activeUserAccounts);
        }

        public async Task<Result<ExchangeAccountInfoOutputModel>> GetDetailsByUserId(string userId, int accountId)
        {
            var ownerId = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId).Id;

            var exchangeAccount =
                await this.dbContext.ExchangeAccounts
                    .Include(ea => ea.Type)
                    .Include(ea => ea.SentTransactions)
                    .ThenInclude(st => st.ReceiverAccount)
                    .Include(ea => ea.ReceivedTransactions)
                    .ThenInclude(rt => rt.SenderAccount)
                    .FirstOrDefaultAsync(ea => ea.Id == accountId && ea.OwnerId == ownerId);

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