namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Controllers;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.ExchangeAccountTypes;
    using System.Threading.Tasks;

    public class ExchangeAccountTypesController : BaseApiController
    {
        private readonly IExchangeAccountTypeService exchangeAccountTypes;

        public ExchangeAccountTypesController(IExchangeAccountTypeService exchangeAccountTypes)
        {
            this.exchangeAccountTypes = exchangeAccountTypes;
        }

        [HttpGet]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll() => this.Ok(await this.exchangeAccountTypes.GetAll());
    }
}
