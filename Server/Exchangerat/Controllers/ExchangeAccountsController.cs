﻿namespace Exchangerat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.ExchangeAccounts;
    using Services.Contracts.Identity;
    using System.Threading.Tasks;

    public class ExchangeAccountsController : BaseApiController
    {
        private readonly ICurrentUserService currentUser;

        private readonly IExchangeAccountService exchangeAccounts;

        public ExchangeAccountsController(ICurrentUserService currentUser, IExchangeAccountService exchangeAccounts)
        {
            this.exchangeAccounts = exchangeAccounts;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route(nameof(GetByUser))]
        public async Task<IActionResult> GetByUser()
        {
            var userAccounts = await this.exchangeAccounts.GetByUserId(this.currentUser.Id);

            return this.Ok(userAccounts);
        }

        [HttpGet]
        [Route(nameof(GetAccountTransactions))]
        public async Task<IActionResult> GetAccountTransactions(int accountId)
        {
            var result = await this.exchangeAccounts.GetDetailsByUserId(this.currentUser.Id, accountId);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }
    }
}
