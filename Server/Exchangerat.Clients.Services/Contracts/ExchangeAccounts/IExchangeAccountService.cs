namespace Exchangerat.Clients.Services.Contracts.ExchangeAccounts
{
    using Exchangerat.Clients.Models.ExchangeAccounts;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        Task<Result<ICollection<UserExchangeAccountOutputModel>>> GetByUserId(string userId);

        Task<Result<ICollection<UserExchangeAccountBaseInfoOutputModel>>> GetActiveByUserForTransaction(string userId);

        Task<Result<ExchangeAccountInfoOutputModel>> GetDetailsByUserId(string userId, int accountId);
    }
}
