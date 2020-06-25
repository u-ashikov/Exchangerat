namespace Exchangerat.Services.Implementations.ExchangeAccounts
{
    using Data;
    using Exchangerat.Models.ExchangeAccount;
    using Exchangerat.Services.Contracts.ExchangeAccounts;
    using Exchangerat.Services.Models.Common;
    using Microsoft.EntityFrameworkCore;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ExchangeAccountService : IExchangeAccountService
    {
        private readonly ExchangeratDbContext dbContext;

        public ExchangeAccountService(ExchangeratDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<Result<ICollection<UserExchangeAccountOutputModel>>> GetByUserId(string userId)
        {
            var userAccounts = await this.dbContext
                .ExchangeAccounts
                .Where(ea => ea.OwnerId == userId)
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
    }
}