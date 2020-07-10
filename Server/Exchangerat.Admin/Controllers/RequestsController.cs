namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Messages.Admin;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Requests;
    using System.Threading.Tasks;

    public class RequestsController : AdminController
    {
        private readonly IRequestService requests;

        private readonly IBus publisher;

        public RequestsController(IRequestService requests, IBus publisher)
        {
            this.requests = requests;
            this.publisher = publisher;
        }

        public async Task<IActionResult> GetAll()
        {
            var allRequests = await this.requests.GetAll();

            return this.View(allRequests);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int requestId, string userId, string requestType, int? accountId)
        {
            // TODO: Should I send the token as a header?

            await this.publisher.Publish(new RequestApprovedMessage() { RequestId = requestId, UserId = userId, AccountId = accountId, RequestType = requestType});

            return this.RedirectToAction(nameof(GetAll));
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int requestId)
        {
            await this.publisher.Publish(new RequestCancelledMessage() { RequestId = requestId });

            return this.RedirectToAction(nameof(GetAll));
        }
    }
}
