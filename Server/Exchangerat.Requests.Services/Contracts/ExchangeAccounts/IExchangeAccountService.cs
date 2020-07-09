﻿namespace Exchangerat.Requests.Services.Contracts.ExchangeAccounts
{
    using Refit;
    using System.Threading.Tasks;

    public interface IExchangeAccountService
    {
        [Get("/ExchangeAccounts/IsOwner")]
        Task<bool> IsOwner([Query] int accountId, [Query] string userId);
    }
}
