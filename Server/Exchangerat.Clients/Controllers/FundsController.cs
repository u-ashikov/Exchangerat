namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Models.Funds;
    using Services.Contracts.Funds;
    using System.Threading.Tasks;

    public class FundsController : BaseApiController
    {
        private readonly IFundService funds;

        private readonly ICurrentUserService currentUser;

        public FundsController(IFundService funds, ICurrentUserService currentUser)
        {
            this.funds = funds;
            this.currentUser = currentUser;
        }

        [HttpPost]
        [Route(nameof(Add))]
        public async Task<IActionResult> Add([FromBody]FundInputModel model)
        {
            var result = await this.funds.Add(model, this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok();
        }
    }
}
