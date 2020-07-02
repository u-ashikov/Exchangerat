namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Transactions;
    using Services.Contracts.Transactions;
    using System.Threading.Tasks;

    public class TransactionsController : BaseApiController
    {
        private readonly ITransactionService transactions;

        private readonly ICurrentUserService currentUser;

        public TransactionsController(ITransactionService transactions, ICurrentUserService currentUser)
        {
            this.transactions = transactions;
            this.currentUser = currentUser;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] TransactionInputModel model)
        {
            var result = await this.transactions.Create(model, this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok("Transaction successfully created.");
        }
    }
}
