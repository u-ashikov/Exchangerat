namespace Exchangerat.Requests.Gateway.Controllers
{
    using Constants;
    using Exchangerat.Controllers;
    using Exchangerat.Requests.Gateway.Models.Requests;
    using Exchangerat.Requests.Gateway.Services.Requests;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Services.Clients;
    using Services.ExchangeAccounts;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class RequestsController : BaseApiController
    {
        private readonly IRequestService requests;

        private readonly IClientService clients;

        private readonly IExchangeAccountService exchangeAccounts;

        public RequestsController(IRequestService requests, IClientService clients, IExchangeAccountService exchangeAccounts)
        {
            this.requests = requests;
            this.clients = clients;
            this.exchangeAccounts = exchangeAccounts;
        }

        [HttpGet]
        [Authorize(Roles = "Administrator")]
        [Route(nameof(GetAll))]
        public async Task<IActionResult> GetAll([FromQuery]int? status, [FromQuery]int page = WebConstants.FirstPage)
        {
            var allRequests = await this.requests.GetAll(status, page);

            var userIds = new HashSet<string>(allRequests.Requests.Select(r => r.UserId));
            var clientsByRequests = await this.clients.GetAllByUserIds(userIds);

            var result = new RequestListingOutputModel()
            {
                TotalItems = allRequests.TotalItems,
                Requests = new List<ClientRequestOutputModel>()
            };

            foreach (var request in allRequests.Requests)
            {
                var clientInfo = clientsByRequests.FirstOrDefault(c => c.UserId == request.UserId);

                var clientRequest = new ClientRequestOutputModel()
                {
                    Id = request.Id,
                    UserId = request.UserId,
                    IssuedAt = request.IssuedAt,
                    AccountId = request.AccountId,
                    RequestType = request.RequestType,
                    Status = request.Status,
                    AccountTypeId = request.AccountTypeId,
                    ClientFirstName = clientInfo?.FirstName,
                    ClientLastName = clientInfo?.LastName
                };

                result.Requests.Add(clientRequest);
            }

            return this.Ok(result);
        }

        [HttpGet]
        [Route(nameof(GetMy))]
        public async Task<IActionResult> GetMy()
        {
            var requests = await this.requests.GetMy();

            if (requests == null || !requests.Any())
            {
                return this.Ok(requests);
            }

            var accounts =
                await this.exchangeAccounts.GetByIds(requests.Where(r => r.AccountId.HasValue)
                    .Select(r => r.AccountId.Value));

            var requestsWithAccountCreation = requests.Where(r => r.AccountId.HasValue).ToList();

            foreach (var request in requestsWithAccountCreation)
            {
                request.AccountIdentityNumber = accounts.FirstOrDefault(a => a.Id == request.AccountId.Value)?.IdentityNumber;
            }

            return this.Ok(requests);
        }
    }
}
