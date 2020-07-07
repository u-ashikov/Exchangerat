namespace Exchangerat.Requests.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ExchangeratRequestsController : BaseApiController
    {
        private readonly IRequestService requests;

        public ExchangeratRequestsController(IRequestService requests)
        {
            this.requests = requests;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var result = await this.requests.GetAll();

            return this.Ok(result.Data);
        }
    }
}
