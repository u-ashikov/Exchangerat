using Microsoft.AspNetCore.Connections.Features;

namespace Exchangerat.Clients.Services.Implementations.Transactions
{
    using Data;
    using Exchangerat.Clients.Data.Models;
    using Exchangerat.Clients.Models.Transactions;
    using Exchangerat.Clients.Services.Contracts.Transactions;
    using Exchangerat.Services.Identity;
    using Infrastructure;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class TransactionService : ITransactionService
    {
        private readonly ClientsDbContext dbContext;

        public TransactionService(ClientsDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result> Create(TransactionInputModel model, string userId)
        {
            var client = this.dbContext.Clients.FirstOrDefault(c => c.UserId == userId);

            if (client == null)
            {
                return Result.Failure("Transaction cannot be finished.");
            }

            var senderAccount = await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(sa =>
                sa.IdentityNumber == model.SenderAccount && sa.OwnerId == client.Id);

            var errors = new List<string>();

            if (senderAccount == null)
            {
                errors.Add("The Sender Account does not exist.");
            }

            if (senderAccount != null && senderAccount.Balance < model.Amount)
            {
                errors.Add("Insufficient amount for this transaction.");
            }

            var receiverAccount =
                await this.dbContext.ExchangeAccounts.FirstOrDefaultAsync(ra =>
                    ra.IdentityNumber == model.ReceiverAccount);

            if (receiverAccount == null)
            {
                errors.Add("The Receiver Account does not exist.");
            }

            if (senderAccount != null && receiverAccount != null &&
                senderAccount.IdentityNumber == receiverAccount.IdentityNumber)
            {
                errors.Add("You cannot create transaction between same accounts.");
            }

            if (errors.Any())
            {
                return Result.Failure(errors);
            }

            var transaction = new Transaction()
            {
                SenderAccountId = senderAccount.Id,
                ReceiverAccountId = receiverAccount.Id,
                Amount = model.Amount,
                Description = model.Description,
                IssuedAt = DateTime.UtcNow
            };

            senderAccount.Balance -= model.Amount;
            receiverAccount.Balance += model.Amount;

            this.dbContext.Transactions.Add(transaction);

            await this.dbContext.SaveChangesAsync();

            return Result.Success;
        }
    }
}
