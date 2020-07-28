namespace Exchangerat.Admin.Controllers
{
    using Constants;
    using Exchangerat.Data.Models;
    using Exchangerat.Messages.Admin;
    using Exchangerat.Models.Pagination;
    using Exchangerat.Services.Common;
    using MassTransit;
    using Microsoft.AspNetCore.Mvc;
    using Models.Requests;
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

        public async Task<IActionResult> GetAll(SearchFormModel searchForm, int page = WebConstants.FirstPage)
        {
            if (page < WebConstants.FirstPage)
            {
                page = WebConstants.FirstPage;
            }

            var allRequests = await this.requests.GetAll(searchForm.Status, page);

            var pagination = new PaginationViewModel()
            {
                PageSize = WebConstants.ItemsPerPage,
                TotalElements = allRequests.TotalItems,
                CurrentPage = page
            };

            if (page > pagination.TotalPages)
            {
                pagination.CurrentPage = pagination.TotalPages;
            }

            var result = new RequestListingViewModel()
            {
                Requests = allRequests.Requests,
                Pagination = pagination,
                Search = searchForm
            };

            return this.View(result);
        }

        [HttpPost]
        public async Task<IActionResult> Approve(int requestId, string userId, string requestType, int? accountId, int? accountTypeId)
        {
            var messageData = new RequestApprovedMessage()
            {
                RequestId = requestId,
                UserId = userId,
                AccountId = accountId,
                RequestType = requestType,
                AccountTypeId = accountTypeId
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
            var messageData = new RequestCancelledMessage()
            {
                RequestId = requestId
            };

            var message = new Message(messageData);

            await this.messages.SaveMessage(message);

            await this.publisher.Publish(messageData);

            await this.messages.MarkMessageAsPublished(message.Id);

            return this.RedirectToAction(nameof(GetAll));
        }
    }
}
