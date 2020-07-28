namespace Exchangerat.Requests.Controllers
{
    using Constants;
    using Data.Enums;
    using Exchangerat.Controllers;
    using Exchangerat.Requests.Models.Models.Requests;
    using Exchangerat.Requests.Services.Contracts.Requests;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ExchangeratRequestsController : BaseApiController
    {
        private readonly IRequestService requests;

        private readonly ICurrentUserService currentUser;

        public ExchangeratRequestsController(IRequestService requests, ICurrentUserService currentUser)
        {
            this.requests = requests;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll([FromQuery]Status? status, [FromQuery]int page = WebConstants.FirstPage)
        {
            var result = await this.requests.GetAll(status, page);

            return this.Ok(result.Data);
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody]CreateRequestFormModel model)
        {
            var result = await this.requests.Create(model, this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok();
        }

        [HttpGet]
        [Route(nameof(GetMy))]
        public async Task<IActionResult> GetMy()
        {
            var result = await this.requests.GetMy(this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }
    }
}
