namespace Exchangerat.Clients.Controllers
{
    using Constants;
    using Exchangerat.Clients.Models.Clients;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Controllers;
    using Exchangerat.Services.Identity;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class ClientsController : BaseApiController
    {
        private readonly IClientService clients;

        private readonly ICurrentUserService currentUser;

        public ClientsController(IClientService clients, ICurrentUserService currentUser)
        {
            this.clients = clients;
            this.currentUser = currentUser;
        }

        [HttpPost]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody]ClientInputModel model)
        {
            var result = await this.clients.Create(model, this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpGet]
        [Route(nameof(GetClientId))]
        public async Task<IActionResult> GetClientId()
        {
            var result = await this.clients.GetIdByUserId(this.currentUser.Id);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok(result.Data);
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route(nameof(GetAllByUserIds))]
        public async Task<IActionResult> GetAllByUserIds([FromQuery]IEnumerable<string> userIds)
        {
            var result = await this.clients.GetAllByUserIds(userIds);

            if (!result.Succeeded)
            {
                return this.BadRequest(WebConstants.DefaultErrorMessage);
            }

            return this.Ok(result.Data);
        }
    }
}
