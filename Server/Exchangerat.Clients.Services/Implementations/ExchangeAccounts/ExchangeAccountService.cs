namespace Exchangerat.Clients.Services.Implementations.ExchangeAccounts
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.ExchangeAccounts;
    using Exchangerat.Clients.Models.Transactions;
    using Exchangerat.Clients.Services.Contracts.ExchangeAccounts;
    using Exchangerat.Services.ExchangeAccounts;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using static Exchangerat.Clients.Common.Constants.WebConstants;

    public class ExchangeAccountService : IExchangeAccountService
    {
        private readonly ClientsDbContext dbContext;

        private readonly IIdentityNumberGenerator identityNumberGenerator;

        public ExchangeAccountService(ClientsDbContext dbContext, IIdentityNumberGenerator identityNumberGenerator)
        {
            this.dbContext = dbContext;
            this.identityNumberGenerator = identityNumberGenerator;
        }

        public async Task<Result<ICollection<ClientExchangeAccountOutputModel>>> GetMy(string userId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ICollection<ClientExchangeAccountOutputModel>>.Failure(Messages.YouAreNotAClient);
            }

            var userAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == owner.Id)
                .AsNoTracking()
                .Select(ea => new ClientExchangeAccountOutputModel()
                {
                    Id = ea.Id,
                    Balance = ea.Balance,
                    IdentityNumber = ea.IdentityNumber,
                    Type = ea.Type.Name,
                    IsActive = ea.IsActive,
                    CreatedAt = ea.CreatedAt
                })
                .ToListAsync();

            return Result<ICollection<ClientExchangeAccountOutputModel>>.SuccessWith(userAccounts);
        }

        public async Task<Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>> GetActiveByClient(
            string userId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>.Failure(Messages.NoAccountsFound);
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

        public async Task<Result<ExchangeAccountDetailsOutputModel>> GetAccountDetails(string userId, int accountId)
        {
            var owner = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (owner == null)
            {
                return Result<ExchangeAccountDetailsOutputModel>.Failure(Messages.YouAreNotAClient);
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
                return Result<ExchangeAccountDetailsOutputModel>.Failure(Messages.AccountNotFound);
            }

            var accountInfo = new ExchangeAccountDetailsOutputModel()
            {
                IdentityNumber = exchangeAccount.IdentityNumber,
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

            return Result<ExchangeAccountDetailsOutputModel>.SuccessWith(accountInfo);
        }

        public async Task<bool> IsOwner(int accountId, string userId)
        {
            var existingAccount = await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(ea => ea.Id == accountId);

            if (existingAccount == null)
            {
                return false;
            }

            var owner = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (owner == null)
            {
                return false;
            }

            return existingAccount.OwnerId == owner.Id;
        }

        // TODO: Add exchange account type from the user selection.
        public async Task Create(string userId)
        {
            var existingClient = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingClient == null)
            {
                return;
            }

            var exchangeAccountType = await this.dbContext.ExchangeAccountTypes.FirstOrDefaultAsync(eat => eat.Name == "Standard");

            if (exchangeAccountType == null)
            {
                return;
            }

            var exchangeAccount = new ExchangeAccount()
            {
                OwnerId = existingClient.Id,
                CreatedAt = DateTime.UtcNow,
                IsActive = true,
                Type = exchangeAccountType
            };

            var identityNumber = this.identityNumberGenerator.Generate();

            var accountExists = this.dbContext.ExchangeAccounts.Any(ea => ea.IdentityNumber == identityNumber);

            while (accountExists)
            {
                identityNumber = this.identityNumberGenerator.Generate();

                accountExists = this.dbContext.ExchangeAccounts.Any(ea => ea.IdentityNumber == identityNumber);
            }

            exchangeAccount.IdentityNumber = identityNumber;

            this.dbContext.ExchangeAccounts.Add(exchangeAccount);

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Block(string userId, int accountId)
        {
            var existingClient = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingClient == null)
            {
                return;
            }

            var existingAccount = await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(ea => ea.Id == accountId && ea.OwnerId == existingClient.Id);

            if (existingAccount == null)
            {
                return;
            }

            existingAccount.IsActive = false;

            await this.dbContext.SaveChangesAsync();
        }

        public async Task Delete(string userId, int accountId)
        {
            var existingClient = await this.dbContext.Clients.FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingClient == null)
            {
                return;
            }

            var existingAccount = await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(ea => ea.Id == accountId && ea.OwnerId == existingClient.Id);

            if (existingAccount == null)
            {
                return;
            }

            existingAccount.IsActive = false;
            existingAccount.ClosedAt = DateTime.UtcNow;

            await this.dbContext.SaveChangesAsync();
        }
    }
}