namespace Exchangerat.Clients.Services.Contracts.ExchangeAccountTypes
{
    using Exchangerat.Clients.Models.ExchangeAccountTypes;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IExchangeAccountTypeService
    {
        Task<IEnumerable<ExchangeAccountTypeOutputModel>> GetAll();
    }
}
