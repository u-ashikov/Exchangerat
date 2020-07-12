namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.ExchangeAccounts;
    using System.Collections.Generic;
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
        [Route(nameof(GetMy))]
        public async Task<IActionResult> GetMy()
        {
            var result = await this.exchangeAccounts.GetMy(this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpGet]
        [Route(nameof(GetAccountTransactions))]
        public async Task<IActionResult> GetAccountTransactions(int accountId)
        {
            var result = await this.exchangeAccounts.GetAccountDetails(this.currentUser.Id, accountId);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpGet]
        [Route(nameof(GetActiveByClient))]
        public async Task<IActionResult> GetActiveByClient()
        {
            var result = await this.exchangeAccounts.GetActiveByClient(this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpGet]
        [Route(nameof(IsOwner))]
        public async Task<IActionResult> IsOwner(int accountId, string userId)
            => this.Ok(await this.exchangeAccounts.IsOwner(accountId, userId));

        [HttpGet]
        [Route(nameof(GetByIds))]
        public async Task<IActionResult> GetByIds([FromQuery]IEnumerable<int> ids)
        {
            var result = await this.exchangeAccounts.GetByIds(ids);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }
    }
}
