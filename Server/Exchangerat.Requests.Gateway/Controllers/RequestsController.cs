namespace Exchangerat.Requests.Gateway.Controllers
{
    using Exchangerat.Controllers;
    using Exchangerat.Requests.Gateway.Models.Requests;
    using Exchangerat.Requests.Gateway.Services.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Clients;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestsController : BaseApiController
    {
        private readonly IRequestService requests;

        private readonly IClientService clients;

        public RequestsController(IRequestService requests, IClientService clients)
        {
            this.requests = requests;
            this.clients = clients;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll()
        {
            var requests = await this.requests.GetAll();

            var userIds = new HashSet<string>(requests.Select(r => r.UserId));
            var clients = await this.clients.GetAllByUserIds(userIds);

            var result = new RequestListingOutputModel();

            foreach (var request in requests)
            {
                var clientInfo = clients.FirstOrDefault(c => c.UserId == request.UserId);

                var clientRequest = new ClientRequestOutputModel()
                {
                    Id = request.Id,
                    UserId = request.UserId,
                    IssuedAt = request.IssuedAt,
                    AccountId = request.AccountId,
                    RequestType = request.RequestType,
                    Status = request.Status,
                    ClientFirstName = clientInfo?.FirstName,
                    ClientLastName = clientInfo?.LastName
                };

                result.Requests.Add(clientRequest);
            }

            return this.Ok(result);
        }
    }
}
