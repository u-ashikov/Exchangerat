namespace Exchangerat.Clients.Services.Contracts.ExchangeAccounts
{
    using Exchangerat.Clients.Models.ExchangeAccounts;
    using Infrastructure;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        Task<Result<ICollection<ClientExchangeAccountOutputModel>>> GetMy(string userId);

        Task<Result<ICollection<ClientExchangeAccountBaseInfoOutputModel>>> GetActiveByClient(string userId);

        Task<Result<ICollection<ExchangeAccountBaseInfoModel>>> GetByIds(IEnumerable<int> ids);

        Task<Result<ExchangeAccountDetailsOutputModel>> GetAccountDetails(string userId, int accountId);

        Task<bool> IsOwner(int accountId, string userId);

        Task Create(string userId, int accountTypeId);

        Task Block(string userId, int accountId);

        Task Delete(string userId, int accountId);
    }
}
