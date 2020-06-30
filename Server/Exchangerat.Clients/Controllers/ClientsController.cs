namespace Exchangerat.Clients.Controllers
{
    using Exchangerat.Clients.Models.Clients;
    using Exchangerat.Clients.Services.Contracts.Clients;
    using Exchangerat.Controllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;

    public class ClientsController : BaseApiController
    {
        private readonly IClientService clients;

        public ClientsController(IClientService clients)
        {
            this.clients = clients;
        }

        [HttpPost]
        [Authorize]
        [Route(nameof(Create))]
        public async Task<IActionResult> Create([FromBody]ClientInputModel model)
        {
            var result = await this.clients.Create(model);

            if (!result.Succeeded)
            {
                return this.BadRequest(result.Errors);
            }

            return this.Ok();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetClientId()
        {
            return this.Ok(10);
        }
    }
}
