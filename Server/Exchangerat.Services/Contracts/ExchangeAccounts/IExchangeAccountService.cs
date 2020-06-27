namespace Exchangerat.Services.Contracts.ExchangeAccounts
{
    using Exchangerat.Models.ExchangeAccount;
    using Exchangerat.Services.Models.Common;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        Task<Result<ICollection<UserExchangeAccountOutputModel>>> GetByUserId(string userId);

        Task<Result<ExchangeAccountInfoOutputModel>> GetDetailsByUserId(string userId, int accountId);
    }
}
