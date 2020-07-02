namespace Exchangerat.Clients.Services.Contracts.ExchangeAccounts
{
    using Exchangerat.Clients.Models.ExchangeAccounts;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        Task<Result<ICollection<ClientExchangeAccountOutputModel>>> GetMy(string userId);

        Task<Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>> GetActiveByClientForTransaction(string userId);

        Task<Result<ExchangeAccountInfoOutputModel>> GetAccountDetails(string userId, int accountId);
    }
}
