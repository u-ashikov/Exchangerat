namespace Exchangerat.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using Models.Transactions;
    using Services.Contracts.Transactions;
    using System.Threading.Tasks;

    public class TransactionsController : BaseApiController
    {
        private readonly ITransactionService transactions;

        public TransactionsController(ITransactionService transactions)
        {
            this.transactions = transactions;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody] TransactionInputModel model)
        {
            var result = await this.transactions.Create(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok("Transaction successfully created.");
        }
    }
}
