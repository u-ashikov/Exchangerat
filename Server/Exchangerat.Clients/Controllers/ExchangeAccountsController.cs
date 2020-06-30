namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.ExchangeAccounts;
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
        [Route(nameof(GetActiveByUserForTransaction))]
        public async Task<IActionResult> GetActiveByUserForTransaction()
        {
            var userActiveExchangeAccounts = await this.exchangeAccounts.GetActiveByUserForTransaction(this.currentUser.Id);

            return this.Ok(userActiveExchangeAccounts);
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
