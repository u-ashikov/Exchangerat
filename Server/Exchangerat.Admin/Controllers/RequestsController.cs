namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Messages.Admin;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Requests;
    using System.Threading.Tasks;

    public class RequestsController : AdminController
    {
        private readonly IRequestService requestService;

        private readonly IBus publisher;

        public RequestsController(IRequestService requestService, IBus publisher)
        {
            this.requestService = requestService;
            this.publisher = publisher;
        }

        public async Task<IActionResult> GetAll()
        {
            var result = await this.requestService.GetAll();

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int requestId, string userId, int? accountId)
        {
            // TODO: Should I send the token as a header?

            await this.publisher.Publish(new RequestApprovedMessage() { RequestId = requestId, UserId = userId, AccountId = accountId });

            return this.RedirectToAction(nameof(GetAll));
        }
    }
}
