namespace Exchangerat.Admin.Controllers
{
    using Exchangerat.Data.Models;
    using Exchangerat.Messages.Admin;
    using Exchangerat.Services.Common;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Services.Contracts.Requests;
    using System.Threading.Tasks;

    public class RequestsController : AdminController
    {
        private readonly IRequestService requests;

        private readonly IMessageService messages;

        private readonly IBus publisher;

        public RequestsController(IRequestService requests, IBus publisher, IMessageService messages)
        {
            this.requests = requests;
            this.publisher = publisher;
            this.messages = messages;
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

            var messageData = new RequestApprovedMessage()
            {
                RequestId = requestId,
                UserId = userId,
                AccountId = accountId,
                RequestType = requestType
            };

            var message = new Message(messageData);

            await this.messages.SaveMessage(message);

            await this.publisher.Publish(messageData);

            await this.messages.MarkMessageAsPublished(message.Id);

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
