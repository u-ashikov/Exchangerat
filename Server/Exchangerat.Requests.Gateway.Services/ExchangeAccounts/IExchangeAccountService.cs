namespace Exchangerat.Requests.Gateway.Services.ExchangeAccounts
{
    using Exchangerat.Requests.Gateway.Models.ExchangeAccounts;
    using Refit;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        [Get("/ExchangeAccounts/GetByIds")]
        Task<IEnumerable<ExchangeAccountBaseInfoOutputModel>> GetByIds([Query(CollectionFormat.Multi)] IEnumerable<int> ids);
    }
}
